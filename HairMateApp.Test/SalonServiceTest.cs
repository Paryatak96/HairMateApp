using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HairMateApp.Application.Interfaces;
using HairMateApp.Application.Services;
using HairMateApp.Application.ViewModels.Salon;
using HairMateApp.Domain.Interface;
using HairMateApp.Domain.Model;
using HairMateApp.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class SalonServiceTests
{
    private readonly Mock<ISalonRepository> _salonRepositoryMock;
    private readonly DbContextOptions<Context> _dbContextOptions;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly IMapper _mapper;
    private readonly ISalonService _salonService;

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
}
