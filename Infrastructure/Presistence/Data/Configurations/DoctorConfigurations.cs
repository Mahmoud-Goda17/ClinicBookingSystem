using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configurations
{
    public class DoctorConfigurations : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Specialty).IsRequired().HasMaxLength(100);
            builder.Property(d => d.PhoneNumber).IsRequired().HasMaxLength(11);
            builder.Property(d => d.Address).IsRequired().HasMaxLength(200);
            builder.Property(d => d.Email).HasMaxLength(100);
            builder.Property(d => d.ConsultationFee).IsRequired().HasColumnType("decimal(18,2)");
            builder.HasIndex(d => new { d.Email, d.PhoneNumber })
                   .IsUnique();
            builder.HasMany(d => d.Appointments)
                   .WithOne(a => a.Doctor)
                   .HasForeignKey(a => a.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(d => d.DoctorSchedules)
                   .WithOne(ds => ds.Doctor)
                   .HasForeignKey(ds => ds.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
