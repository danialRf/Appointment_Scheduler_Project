﻿using Appointment_Scheduler_Project.Applications.Repository;
using Appointment_Scheduler_Project.Domain.Entities;
using Appointment_Scheduler_Project.Domain.Enums;
using Appointment_Scheduler_Project.Persistences.EF;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace Appointment_Scheduler_Project.Persistences.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApScDbContext _context;

        public AppointmentRepository(ApScDbContext dbcontext)
        {
            _context = dbcontext;
        }
        public async Task<IList<Appointment>> GetAll()
        {
            return await _context.Appointments.ToListAsync();
        }
        public async Task<Appointment> GetById(int id)
        {
            return await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<Appointment>> GetAllFreeAppointments()
        {
            return await _context.Appointments
                .Where(x => x.IsExpired == false)
                .ToListAsync();
        }


        public async Task<int> Create(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment.Id;
        }

        public async Task<int> Delete(int id)
        {
            _context.Remove(new Appointment { Id = id });
            await _context.SaveChangesAsync();
            return id;
        }


        public Task<bool> DoesExist(int id)
        {
            var result = _context.Appointments.AnyAsync(x => x.Id == id);
            return result;
        }

        public async Task<int> Update(Appointment model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task<bool> Isvalid(int appointmentId)
        {
            var result = _context.Appointments.AnyAsync(x => x.IsExpired == false);
            return await result;
        }

        public async Task<List<Appointment>> GetAppointmentsBeforeDate(DateTime date)
        {
            return await _context.Appointments
                        .Where(appointment => appointment.AppointmentDate < date)
                        .ToListAsync();
        }

        public async Task<int> GetAppointmentIdByDate(DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.AppointmentDate == date)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();
        }
    }
}
