using HairMateApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Domain.Interface
{
    public interface ISalonRepository
    {
        IQueryable<Salon> GetAllSalons();

        // Metoda do pobrania salonu według ID
        Task<Salon> GetSalonByIdAsync(int salonId);

        // Metoda do stworzenia nowego salonu
        Task<Salon> CreateSalonAsync(Salon salon);

        // Metoda do aktualizacji istniejącego salonu
        Task<bool> UpdateSalonAsync(Salon salon);

        // Metoda do usunięcia salonu według ID
        Task<bool> DeleteSalonAsync(int salonId);

        // Metoda do pobrania salonów według województwa
        Task<IEnumerable<Salon>> GetSalonsByProvinceAsync(string province);

        // Metoda do pobrania salonów według rodzaju (męski, damski)
        Task<IEnumerable<Salon>> GetSalonsByTypeAsync(string type);

        // Metoda do wyszukiwania salonów według nazwy
        Task<IEnumerable<Salon>> SearchSalonsByNameAsync(string name);

        // Metoda do zarządzania usługami w salonie
        Task<bool> ManageServicesAsync(int salonId, IEnumerable<Service> services);

        // Metoda do dodania opinii do salonu
        Task<bool> AddReviewAsync(int salonId, Review review);

        // Metoda do odpowiedzi na opinię w salonie
        Task<bool> RespondToReviewAsync(int reviewId, string response);



        Task<IEnumerable<Appointment>> GetAppointmentsBySalonIdAsync(int salonId);
        public Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(int salonId, DateTime date);
        public Task AddAppointmentsAsync(IEnumerable<Appointment> appointments);
        Task<bool> BookAppointmentAsync(int appointmentId);
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);




        Task<List<Salon>> GetSalonsByRegionAndGenderAsync(string region, string gender);
        Task<List<Salon>> GetAllSalonsAsync();
        Task<IEnumerable<Appointment>> GetBookedAppointmentsAsync(string userId);
        Task<bool> CancelAppointmentAsync(int appointmentId);
        Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime);
        Task<bool> CompleteAppointmentAsync(int appointmentId);
        Task<bool> CreateAppointmentAsync(Appointment appointment);
        Task<bool> AddReviewAsync(Review review);

    }
}