using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Drones.DAL.DbContext;

public class DroneDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    
        public DroneDbContext(DbContextOptions<DroneDbContext> options)
            : base(options)
        {
        }

        public DbSet<Drone> Drones { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<BatteryLog> BatteryLogs { get; set; }
        public DbSet<LoadedMedication> LoadedMedications { get; set; }
        
        
}