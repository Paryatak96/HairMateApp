using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection.Emit;
using HairMateApp.Domain.Model;

namespace HairMateApp.Infrastructure
{
    public class Context : IdentityDbContext
    {
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public Context(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Salon>()
                .HasMany(s => s.Services)
                .WithOne(s => s.Salon)
                .HasForeignKey(s => s.SalonId);

            modelBuilder.Entity<Salon>()
                .HasMany(s => s.Reviews)
                .WithOne(r => r.Salon)
                .HasForeignKey(r => r.SalonId);

            // Dodaj dodatkową konfigurację dla ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.Region).HasMaxLength(100);
                entity.Property(e => e.ProfilePicture);
            });
        }
    }
}
