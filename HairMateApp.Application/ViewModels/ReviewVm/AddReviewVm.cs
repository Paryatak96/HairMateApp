using AutoMapper;
using HairMateApp.Application.Mapping;
using HairMateApp.Application.ViewModels.Salon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Application.ViewModels.ReviewVm
{
    public class AddReviewVm
    {
        public int SalonId { get; set; }
        public int Rating { get; set; } // Ocena w skali od 1 do 5
        public string Comment { get; set; }
    }
}
