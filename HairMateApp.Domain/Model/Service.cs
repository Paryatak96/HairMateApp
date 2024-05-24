using HairMateApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Domain.Model
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ServiceType { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
    }
}
