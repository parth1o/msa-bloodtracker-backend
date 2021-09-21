using Microsoft.EntityFrameworkCore;
using msa_bloodtracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace msa_bloodtracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Patient> Patients { get; set; } = default!;
        public DbSet<Bloodtest> Bloodtests { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bloodtest>()
                .HasOne(p => p.Patient)
                .WithMany(s => s.Bloodtests)
                .HasForeignKey(p => p.PatientId);
        }
    }
}
