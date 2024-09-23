using HairMateApp.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HairMateApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult UploadProfilePicture()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);

                var filePath = Path.Combine("wwwroot/userimages", userId + Path.GetExtension(file.FileName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                user.ProfilePicture = "/userimages/" + userId + Path.GetExtension(file.FileName);
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
    }
}
