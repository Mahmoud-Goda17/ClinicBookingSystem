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
    public class DoctorScheduleConfigurations : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.HasKey(ds => ds.Id);
            builder.Property(ds => ds.DayOfWeek)
                   .HasConversion<string>()
                   .IsRequired();
            builder.Property(ds => ds.StartTime).IsRequired();
            builder.Property(ds => ds.EndTime).IsRequired();
            builder.Property(ds => ds.IsAvailable).HasDefaultValue(true);
            builder.HasIndex(ds => new { ds.DoctorId, ds.StartTime,ds.DayOfWeek })
                   .IsUnique();

        }
    }

    
}
