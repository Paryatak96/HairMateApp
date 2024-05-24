using AutoMapper;
using HairMateApp.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Application.ViewModels.Salon
{
    public class SalonForListVm : IMapFrom<HairMateApp.Domain.Model.Salon>
    {
        public int SalonId { get; set; }
        public string Province { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Type { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HairMateApp.Domain.Model.Salon, SalonForListVm>();
        }
    }
}
