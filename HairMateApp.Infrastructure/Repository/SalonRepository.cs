using HairMateApp.Domain.Interface;
using HairMateApp.Domain.Model;
using HairMateApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Infrastructure.Repository
{
    public class SalonRepository : ISalonRepository
    {
        private readonly Context _context;

        public SalonRepository(Context context)
        {
            _context = context;
        }
        public async Task<bool> AddReviewAsync(int salonId, Review review)
        {
            var salon = await _context.Salons.Include(s => s.Reviews).FirstOrDefaultAsync(s => s.SalonId == salonId);
            if (salon == null)
            {
                return false;
            }

            salon.Reviews.Add(review);
            _context.Salons.Update(salon);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Salon> CreateSalonAsync(Salon salon)
        {
            _context.Salons.Add(salon);
            await _context.SaveChangesAsync();
            return salon;
        }

        public async Task<bool> DeleteSalonAsync(int salonId)
        {
            var salon = await _context.Salons.FindAsync(salonId);
            if (salon == null)
            {
                return false;
            }

            _context.Salons.Remove(salon);
            return await _context.SaveChangesAsync() > 0;
        }

        public IQueryable<Salon> GetAllSalons()
        {
            return _context.Salons;
        }

        public async Task<Salon> GetSalonByIdAsync(int salonId)
        {
            return await _context.Salons.FindAsync(salonId);
        }

        public async Task<IEnumerable<Salon>> GetSalonsByProvinceAsync(string province)
        {
            return await _context.Salons.Where(s => s.Province == province).ToListAsync();
        }

        public async Task<IEnumerable<Salon>> GetSalonsByTypeAsync(string type)
        {
            return await _context.Salons.Where(s => s.Type == type).ToListAsync();
        }

        public async Task<bool> ManageServicesAsync(int salonId, IEnumerable<Service> services)
        {
            var salon = await _context.Salons.Include(s => s.Services).FirstOrDefaultAsync(s => s.SalonId == salonId);
            if (salon == null)
            {
                return false;
            }

            salon.Services = services.ToList();
            _context.Salons.Update(salon);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RespondToReviewAsync(int reviewId, string response)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
            {
                return false;
            }

            review.Response = response;
            _context.Reviews.Update(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Salon>> SearchSalonsByNameAsync(string name)
        {
            return await _context.Salons.Where(s => s.Name.Contains(name)).ToListAsync();
        }

        public async Task<bool> UpdateSalonAsync(Salon salon)
        {
            _context.Salons.Update(salon);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentsBySalonIdAsync(int salonId)
        {
            return await _context.Appointments.Where(a => a.SalonId == salonId).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(int salonId, DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.SalonId == salonId && a.Date == date)
                .ToListAsync();
        }
        public async Task AddAppointmentsAsync(IEnumerable<Appointment> appointments)
        {
            await _context.Appointments.AddRangeAsync(appointments);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> BookAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null || appointment.Status != "Available")
            {
                return false;
            }

            appointment.Status = "Booked";
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _context.Appointments
                .Include(a => a.Salon)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);
        }























        public async Task<List<Salon>> GetSalonsByRegionAndGenderAsync(string region, string gender)
        {
            return await _context.Salons
                .Where(s => s.Province == region && s.Type == gender)
                .ToListAsync();
        }

        public async Task<List<Salon>> GetAllSalonsAsync()
        {
            return await _context.Salons.ToListAsync();
        }






        public async Task<IEnumerable<Appointment>> GetBookedAppointmentsAsync(string userId)
        {
            return await _context.Appointments
                .Include(a => a.Salon)
                .Where(a => a.Status == "Booked" && a.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return false;
            }

            appointment.Status = "Cancelled";
            _context.Appointments.Update(appointment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return false;
            }

            appointment.Date = newDate;
            appointment.Time = newTime;
            _context.Appointments.Update(appointment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CompleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return false;
            }

            appointment.Status = "Completed";
            _context.Appointments.Update(appointment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
