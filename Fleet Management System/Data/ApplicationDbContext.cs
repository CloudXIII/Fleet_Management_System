using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleComponent> VehicleComponents { get; set; }
    public DbSet<DriverAssignment> DriverAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VehicleComponent>()
            .HasOne(c => c.Vehicle)
            .WithMany(v => v.Components)
            .HasForeignKey(c => c.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DriverAssignment>()
            .HasOne(a => a.Vehicle)
            .WithMany(v => v.Assignments)
            .HasForeignKey(a => a.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
