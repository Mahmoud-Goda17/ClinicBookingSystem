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
    public class AppointmentConfigurations : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FeePaid)
                   .HasColumnType("decimal(18,2)"); 

            builder.Property(a => a.Status)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(a => a.StartTime).IsRequired();
            builder.Property(a => a.EndTime).IsRequired();


            // Unique Index: Doctor cannot have 2 appointments at the same StartTime
            builder.HasIndex(a => new { a.DoctorId, a.StartTime })
                   .IsUnique();
        }
    }
}
