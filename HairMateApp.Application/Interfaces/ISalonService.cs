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
        ListSalonForListVm GetAllSalonsAsync(int pageSize, int pageNo, string searchString);
        Task<Salon> GetSalonByIdAsync(int salonId);
        Task<Salon> CreateSalonAsync(Salon salon);
        Task<bool> UpdateSalonAsync(Salon salon);
        Task<bool> DeleteSalonAsync(int salonId);
        Task<IEnumerable<Salon>> GetSalonsByProvinceAsync(string province);
        Task<IEnumerable<Salon>> GetSalonsByTypeAsync(string type);
        Task<IEnumerable<Salon>> SearchSalonsByNameAsync(string name);
        Task<bool> ManageServicesAsync(int salonId, IEnumerable<Service> services);
        Task<bool> AddReviewAsync(int salonId, Review review);
        Task<bool> RespondToReviewAsync(int reviewId, string response);
        Task<IEnumerable<Appointment>> GetAppointmentsBySalonIdAsync(int salonId);
        public Task GenerateDailyAppointments(int salonId, DateTime date);
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);







        Task<List<Salon>> GetAllSalonsAsync();
        Task<List<Salon>> GetSalonsByRegionAndGenderAsync(string region, string gender);
        Task<IEnumerable<Appointment>> GetBookedAppointmentsAsync(string userId);
        Task<bool> CancelAppointmentAsync(int appointmentId);
        Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime);
        Task<bool> CompleteAppointmentAsync(int appointmentId);
        Task<bool> BookAppointmentAsync(Appointment appointment);
        Task<bool> AddReviewAsync(Review review);
        Task<decimal> GetAverageRatingAsync(int salonId);
    }
}
