using AutoMapper;
using FluentValidation;
using HairMateApp.Application.Mapping;
using HairMateApp.Domain.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Application.ViewModels.Salon
{
    public class NewSalonVm : IMapFrom<HairMateApp.Domain.Model.Salon>
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
        public IFormFile SalonLogo { get; set; }
        public ICollection<Service> Services { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewSalonVm, HairMateApp.Domain.Model.Salon>().ReverseMap();
        }
    }
}
