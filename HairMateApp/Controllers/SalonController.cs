using HairMateApp.Application.Interfaces;
using HairMateApp.Application.ViewModels.Salon;
using HairMateApp.Domain.Interface;
using HairMateApp.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using HairMateApp.Application.ViewModels.ReviewVm;
using Microsoft.AspNetCore.Authorization;

namespace HairMateApp.Controllers
{
    public class SalonController : Controller
    {
        private readonly ISalonService _salonService;
        private readonly ISalonRepository _salonRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalonController(ISalonService salonService, ISalonRepository salonRepository, UserManager<ApplicationUser> userManager)
        {
            _salonService = salonService;
            _salonRepository = salonRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 10, string searchString = "", string salonGender = null)
        {
            List<Salon> salons;

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var userGender = user.Gender;
                var userRegion = user.Region;

                if (string.IsNullOrEmpty(salonGender))
                {
                    salonGender = userGender;
                }

                salons = await _salonService.GetSalonsByRegionAndGenderAsync(userRegion, salonGender);
            }
            else
            {
                salons = await _salonService.GetAllSalonsAsync();
            }

            var salonList = salons
                .Where(s => string.IsNullOrEmpty(searchString) || s.Name.Contains(searchString))
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize)
                .Select(s => new SalonForListVm
                {
                    SalonId = s.SalonId,
                    Name = s.Name,
                    LogoUrl = s.LogoUrl,
                    Description = s.Description,
                    Province = s.Province,
                    Type = s.Type,
                    City = s.City,
                    Street = s.Street
                }).ToList();

            var model = new ListSalonForListVm
            {
                PageSize = pageSize,
                CurrentPage = pageNo,
                SearchString = searchString,
                Salons = salonList,
                Count = salons.Count
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new NewSalonVm());
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewSalonVm model)
        {
            if (true)
            {
                if (model.SalonLogo != null && model.SalonLogo.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/logos", model.SalonLogo.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.SalonLogo.CopyToAsync(stream);
                    }
                    model.LogoUrl = $"/logos/{model.SalonLogo.FileName}";
                }

                var salon = new Salon
                {
                    Name = model.Name,
                    LogoUrl = model.LogoUrl,
                    Description = model.Description,
                    Type = model.Type,
                    Province = model.Province,
                    City = model.City,
                    Street = model.Street,
                    PostalCode = model.PostalCode,
                    PaymentType = model.PaymentType,
                    Services = model.Services,
                    Reviews = model.Reviews
                };

                await _salonService.CreateSalonAsync(salon);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var salon = await _salonService.GetSalonByIdAsync(id);
            if (salon == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var canEdit = user != null && await _userManager.IsInRoleAsync(user, "Admin");
            var canManage = user != null && await _userManager.IsInRoleAsync(user, "Employee");

            var model = new SalonDetailsVm
            {
                SalonId = salon.SalonId,
                Name = salon.Name,
                LogoUrl = salon.LogoUrl,
                Description = salon.Description,
                Type = salon.Type,
                Province = salon.Province,
                City = salon.City,
                Street = salon.Street,
                PostalCode = salon.PostalCode,
                PaymentType = salon.PaymentType,
                Services = salon.Services ?? new List<Service>(),
                Reviews = salon.Reviews ?? new List<Review>(),
                Appointments = salon.Appointments ?? new List<Appointment>(),
                CanEdit = canEdit,
                CanManage = canManage,
                AverageRating = (decimal)(salon.Reviews.Any() ? salon.Reviews.Average(r => r.Rating) : 0)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddService(int salonId, string name, decimal price, string serviceType)
        {
            var salon = await _salonService.GetSalonByIdAsync(salonId);
            if (salon == null)
            {
                return NotFound();
            }

            var service = new Service
            {
                Name = name,
                Price = price,
                ServiceType = serviceType,
                SalonId = salonId
            };

            salon.Services.Add(service);
            await _salonService.UpdateSalonAsync(salon);

            return RedirectToAction("Details", new { id = salonId });
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(int salonId, DateTime date, TimeSpan time)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "IdentityAccount");
            }

            var appointment = new Appointment
            {
                SalonId = salonId,
                Date = date,
                Time = time,
                Status = "Booked",
                UserId = user.Id
            };

            var success = await _salonService.BookAppointmentAsync(appointment);
            if (!success)
            {
                TempData["Error"] = "The selected appointment is no longer available.";
                return RedirectToAction("Details", new { id = salonId });
            }

            return RedirectToAction("BookedAppointments");
        }

        [HttpGet]
        public async Task<IActionResult> BookedAppointments()
        {
            var user = await _userManager.GetUserAsync(User);
            var appointments = await _salonService.GetBookedAppointmentsAsync(user.Id);
            return View(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var success = await _salonService.CancelAppointmentAsync(appointmentId);
            if (!success)
            {
                // Handle error
            }

            return RedirectToAction("BookedAppointments");
        }

        [HttpPost]
        public async Task<IActionResult> RescheduleAppointment(int appointmentId, DateTime newDate, TimeSpan newTime)
        {
            var success = await _salonService.RescheduleAppointmentAsync(appointmentId, newDate, newTime);
            if (!success)
            {
                // Handle error
            }

            return RedirectToAction("BookedAppointments");
        }

        [HttpPost]
        public async Task<IActionResult> CompleteAppointment(int appointmentId)
        {
            var success = await _salonService.CompleteAppointmentAsync(appointmentId);
            if (!success)
            {
                // Handle error
            }

            return RedirectToAction("BookedAppointments");
        }

        [HttpGet]
        public async Task<IActionResult> AddReview(int salonId)
        {
            var model = new AddReviewVm
            {
                SalonId = salonId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var review = new Review
            {
                SalonId = model.SalonId,
                Rating = model.Rating,
                Comment = model.Comment,
                UserId = user.Id,
                UserName = user.FirstName + " " + user.LastName, // Assuming you have a UserName property
            };

            var success = await _salonService.AddReviewAsync(review);
            if (!success)
            {
                TempData["Error"] = "An error occurred while adding the review.";
                return RedirectToAction("Details", new { id = model.SalonId });
            }

            return RedirectToAction("Details", new { id = model.SalonId });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var salon = await _salonService.GetSalonByIdAsync(id);
            if (salon == null)
            {
                return NotFound();
            }

            var model = new NewSalonVm
            {
                SalonId = salon.SalonId,
                Name = salon.Name,
                Description = salon.Description,
                LogoUrl = salon.LogoUrl,
                Type = salon.Type,
                Province = salon.Province,
                City = salon.City,
                Street = salon.Street,
                PostalCode = salon.PostalCode,
                PaymentType = salon.PaymentType
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(NewSalonVm model)
        {

            var salon = await _salonService.GetSalonByIdAsync(model.SalonId);
            if (salon == null)
            {
                return NotFound();
            }

            salon.Name = model.Name;
            salon.Description = model.Description;
            salon.LogoUrl = model.LogoUrl;
            salon.Type = model.Type;
            salon.Province = model.Province;
            salon.City = model.City;
            salon.Street = model.Street;
            salon.PostalCode = model.PostalCode;
            salon.PaymentType = model.PaymentType;

            var success = await _salonService.UpdateSalonAsync(salon);
            if (!success)
            {
                ModelState.AddModelError("", "An error occurred while updating the salon.");
                return View(model);
            }

            return RedirectToAction("Details", new { id = model.SalonId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _salonService.DeleteSalonAsync(id);
            if (!success)
            {
                ModelState.AddModelError("", "An error occurred while deleting the salon.");
                return RedirectToAction("Details", new { id });
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> Appointments(int salonId)
        {
            var salon = await _salonService.GetSalonByIdAsync(salonId);
            if (salon == null)
            {
                return NotFound();
            }

            var appointments = await _salonService.GetAppointmentsBySalonIdAsync(salonId);
            var model = new SalonAppointmentsVm
            {
                SalonId = salon.SalonId,
                SalonName = salon.Name,
                Appointments = appointments.Select(a => new AppointmentVm
                {
                    AppointmentId = a.Id,
                    Date = a.Date,
                    Time = a.Time,
                    Status = a.Status
                }).ToList()
            };

            return View(model);
        }
    }
}
