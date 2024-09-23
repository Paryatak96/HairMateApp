using HairMateApp.Application.ViewModels.Salon;
using HairMateApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Application.Interfaces
{
    public interface ISalonService
    {
        Task<List<Salon>> GetSalonsByRegionAndGenderAsync(string region, string gender);
        Task<List<Salon>> GetAllSalonsAsync();
        Task<Salon> GetSalonByIdAsync(int salonId);
        Task<bool> IsAppointmentAvailableAsync(int salonId, DateTime date, TimeSpan time);
        Task<IEnumerable<Appointment>> GetBookedAppointmentsAsync(string userId);
        Task<bool> CancelAppointmentAsync(int appointmentId);
        Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime);
        Task<bool> CompleteAppointmentAsync(int appointmentId);
        Task<bool> AddReviewAsync(Review review);
        Task<bool> DeleteSalonAsync(int salonId);
        Task<Salon> CreateSalonAsync(Salon salon);
        Task<bool> UpdateSalonAsync(Salon salon);
        Task<bool> BookAppointmentAsync(Appointment appointment);
    }
}
