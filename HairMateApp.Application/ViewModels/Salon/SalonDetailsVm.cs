using AutoMapper;
using HairMateApp.Application.Mapping;
using HairMateApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Application.ViewModels.Salon
{
    public class SalonDetailsVm : IMapFrom<HairMateApp.Domain.Model.Salon>
    {
        public int SalonId { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string PaymentType { get; set; }
        public HairMateApp.Domain.Model.Salon Salon { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Service> Services { get; set; } = new List<Service>(); // Inicjalizacja kolekcji
        public ICollection<Review> Reviews { get; set; } = new List<Review>(); // Inicjalizacja kolekcji
        public bool CanEdit { get; set; }
        public bool CanManage { get; set; }
        public decimal AverageRating { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HairMateApp.Domain.Model.Salon, SalonDetailsVm>();
        }
    }
}
