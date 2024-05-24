using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Application.ViewModels.Salon
{
    public class SalonAppointmentsVm
    {
        public int SalonId { get; set; }
        public string SalonName { get; set; }
        public List<AppointmentVm> Appointments { get; set; }
    }
}
