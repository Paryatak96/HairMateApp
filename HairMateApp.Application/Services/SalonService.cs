using AutoMapper;
using AutoMapper.QueryableExtensions;
using HairMateApp.Application.Interfaces;
using HairMateApp.Application.ViewModels.Salon;
using HairMateApp.Domain.Interface;
using HairMateApp.Domain.Model;
using HairMateApp.Infrastructure;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HairMateApp.Application.Services
{
    public class SalonService : ISalonService
    {
        private readonly ISalonRepository _salonRepository;
        private readonly IMapper _mapper;
        private readonly Context _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalonService(ISalonRepository salonRepository, IMapper mapper, Context context, UserManager<ApplicationUser> userManager)
        {
            _salonRepository = salonRepository;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<Salon> GetSalonByIdAsync(int salonId)
        {
            return await _context.Salons
                .Include(s => s.Reviews) 
                .Include(s => s.Services) 
                .Include(s => s.Appointments)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(s => s.SalonId == salonId);
        }

        public async Task<Salon> CreateSalonAsync(Salon salon)
        {
            return await _salonRepository.CreateSalonAsync(salon);
        }

        public async Task<bool> UpdateSalonAsync(Salon salon)
        {
            return await _salonRepository.UpdateSalonAsync(salon);
        }

        public async Task<bool> DeleteSalonAsync(int salonId)
        {
            return await _salonRepository.DeleteSalonAsync(salonId);
        }

        public async Task<List<Salon>> GetSalonsByRegionAndGenderAsync(string region, string gender)
        {
            return await _salonRepository.GetSalonsByRegionAndGenderAsync(region, gender);
        }

        public async Task<List<Salon>> GetAllSalonsAsync()
        {
            return await _salonRepository.GetAllSalonsAsync();
        }

        public async Task<IEnumerable<Appointment>> GetBookedAppointmentsAsync(string userId)
        {
            return await _salonRepository.GetBookedAppointmentsAsync(userId);
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            return await _salonRepository.CancelAppointmentAsync(appointmentId);
        }

        public async Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime)
        {
            return await _salonRepository.RescheduleAppointmentAsync(appointmentId, newDate, newTime);
        }

        public async Task<bool> CompleteAppointmentAsync(int appointmentId)
        {
            return await _salonRepository.CompleteAppointmentAsync(appointmentId);
        }

        public async Task<bool> BookAppointmentAsync(Appointment appointment)
        {
            return await _salonRepository.CreateAppointmentAsync(appointment);
        }

        public async Task<bool> AddReviewAsync(Review review)
        {
            return await _salonRepository.AddReviewAsync(review);
        }

        public async Task<bool> IsAppointmentAvailableAsync(int salonId, DateTime date, TimeSpan time)
        {
            return await _salonRepository.IsAppointmentAvailableAsync(salonId, date, time);
        }
    }
}
