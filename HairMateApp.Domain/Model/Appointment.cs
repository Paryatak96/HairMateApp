using HairMateApp.Domain.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Domain.Model
{
    public class Appointment
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public string? UserId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Status { get; set; }


        public Salon Salon { get; set; }
        public ApplicationUser User { get; set; }
    }
}