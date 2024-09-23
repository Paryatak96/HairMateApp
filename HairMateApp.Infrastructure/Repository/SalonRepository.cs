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
        public async Task<bool> UpdateSalonAsync(Salon salon)
        {
            _context.Salons.Update(salon);
            return await _context.SaveChangesAsync() > 0;
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
            _context.Appointments.Remove(appointment);
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
        public async Task<bool> IsAppointmentAvailableAsync(int salonId, DateTime date, TimeSpan time)
        {
            return !await _context.Appointments
                .AnyAsync(a => a.SalonId == salonId && a.Date == date && a.Time == time && a.Status == "Booked");
        }
    }
}
