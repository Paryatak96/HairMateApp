using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HairMateApp.Application.Interfaces;
using HairMateApp.Application.Services;
using HairMateApp.Application.ViewModels.Salon;
using HairMateApp.Controllers;
using HairMateApp.Domain.Interface;
using HairMateApp.Domain.Model;
using HairMateApp.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class SalonServiceTests
{
    private readonly Mock<ISalonRepository> _salonRepositoryMock;
    private readonly Mock<ISalonService> _salonServiceMock;
    private readonly DbContextOptions<Context> _dbContextOptions;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly IMapper _mapper;
    private readonly ISalonService _salonService;
    private readonly SalonController _controller;

    public SalonServiceTests()
    {
        _salonRepositoryMock = new Mock<ISalonRepository>();
        _dbContextOptions = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: "HairMateAppTest")
            .Options;
        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Salon, SalonDetailsVm>();
        });

        _mapper = config.CreateMapper();
        var context = new Context(_dbContextOptions);
        _salonService = new SalonService(_salonRepositoryMock.Object, _mapper, context, _userManagerMock.Object);
        _salonServiceMock = new Mock<ISalonService>();

        _controller = new SalonController(_salonServiceMock.Object, _salonRepositoryMock.Object, _userManagerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                        new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"))
                }
            }
        };
    }

    [Fact]
    public async Task GetSalonByIdAsync_ReturnsSalon()
    {
        // Arrange
        var salonId = 1;
        var salon = new Salon
        {
            SalonId = salonId,
            Name = "Test Salon",
            Description = "Test Description",
            LogoUrl = "/path/to/logo.png",
            Type = "Male",
            Province = "Test Province",
            City = "Test City",
            Street = "Test Street",
            PostalCode = "12345",
            PaymentType = "Cash",
            Reviews = new List<Review>
            {
                new Review { Rating = 4, UserId = "1", UserName = "User1" },
                new Review { Rating = 5, UserId = "2", UserName = "User2" }
            },
            Services = new List<Service>
            {
                new Service { Name = "Haircut", Price = 20, ServiceType = "Strzyżenie"},
                new Service { Name = "Shave", Price = 15, ServiceType = "Golenie" }
            }
        };


        using (var context = new Context(_dbContextOptions))
        {
            context.Salons.Add(salon);
            context.SaveChanges();
        }


        Salon result;
        using (var context = new Context(_dbContextOptions))
        {
            var service = new SalonService(_salonRepositoryMock.Object, _mapper, context, _userManagerMock.Object);
            result = await service.GetSalonByIdAsync(salonId);
        }

        Assert.NotNull(result);
        Assert.Equal(salonId, result.SalonId);
        Assert.Equal("Test Salon", result.Name);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal("/path/to/logo.png", result.LogoUrl);
        Assert.Equal("Male", result.Type);
        Assert.Equal("Test Province", result.Province);
        Assert.Equal("Test City", result.City);
        Assert.Equal("Test Street", result.Street);
        Assert.Equal("12345", result.PostalCode);
        Assert.Equal("Cash", result.PaymentType);
        Assert.Equal(2, result.Reviews.Count);
        Assert.Equal(4.5, result.Reviews.Average(r => r.Rating));

        var review1 = result.Reviews.FirstOrDefault(r => r.UserId == "1");
        var review2 = result.Reviews.FirstOrDefault(r => r.UserId == "2");
        Assert.NotNull(review1);
        Assert.NotNull(review2);
        Assert.Equal(4, review1.Rating);
        Assert.Equal("User1", review1.UserName);
        Assert.Equal(5, review2.Rating);
        Assert.Equal("User2", review2.UserName);

        Assert.Equal(2, result.Services.Count);
        var service1 = result.Services.FirstOrDefault(s => s.Name == "Haircut");
        var service2 = result.Services.FirstOrDefault(s => s.Name == "Shave");
        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.Equal(20, service1.Price);
        Assert.Equal(15, service2.Price);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WithSalonDetailsVm()
    {
        // Arrange
        var salon = new Salon
        {
            SalonId = 1,
            Name = "Test Salon",
            Description = "Test Description",
            LogoUrl = "/path/to/logo.png",
            Type = "Male",
            Province = "Test Province",
            City = "Test City",
            Street = "Test Street",
            PostalCode = "12345",
            PaymentType = "Cash",
            Reviews = new List<Review>
            {
                new Review { Rating = 4, UserId = "1", UserName = "User1" },
                new Review { Rating = 5, UserId = "2", UserName = "User2" }
            },
            Services = new List<Service>
            {
                new Service { Name = "Haircut", Price = 20, ServiceType = "Strzyżenie"},
                new Service { Name = "Shave", Price = 15, ServiceType = "Golenie" }
            },
            Appointments = new List<Appointment>
            {
                new Appointment
                {
                    SalonId = 1,
                    Date = DateTime.Now.AddDays(1),
                    Time = new TimeSpan(10, 0, 0), // 10:00 AM
                    Status = "Booked"
                },
                new Appointment
                {
                    SalonId = 1,
                    Date = DateTime.Now.AddDays(2),
                    Time = new TimeSpan(11, 0, 0), // 11:00 AM
                    Status = "Available"
                }
            }
        };

        _salonServiceMock.Setup(s => s.GetSalonByIdAsync(It.IsAny<int>())).ReturnsAsync(salon);

        var user = new ApplicationUser { Id = "1" };
        _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);
        _userManagerMock.Setup(u => u.IsInRoleAsync(user, "Employee")).ReturnsAsync(false);

        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<SalonDetailsVm>(viewResult.Model);
        Assert.Equal(1, model.SalonId);
        Assert.Equal("Test Salon", model.Name);
        Assert.Equal("Test Description", model.Description);
        Assert.Equal("Test Province", model.Province);
        Assert.Equal("Test City", model.City);
        Assert.Equal("Test Street", model.Street);
        Assert.Equal("12345", model.PostalCode);
        Assert.Equal("Cash", model.PaymentType);
        Assert.Equal(2, model.Reviews.Count);
        Assert.Equal(2, model.Services.Count);
        Assert.True(model.CanEdit);
        Assert.False(model.CanManage);
        Assert.Equal(4.5m, model.AverageRating);
    }
}
