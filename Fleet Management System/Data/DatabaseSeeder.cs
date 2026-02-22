using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Vehicles.AnyAsync())
            return;

        var vehicles = new List<Vehicle>
        {
            new() { Plate = "AB12 CDE", Brand = "Ford",       Model = "Transit",   InsuranceExpiry = new DateTime(2026, 3, 15) },
            new() { Plate = "FG34 HIJ", Brand = "Vauxhall",   Model = "Vivaro",    InsuranceExpiry = new DateTime(2025, 11, 30) },
            new() { Plate = "KL56 MNO", Brand = "Mercedes",   Model = "Sprinter",  InsuranceExpiry = new DateTime(2026, 7, 1) },
            new() { Plate = "PQ78 RST", Brand = "Volkswagen", Model = "Crafter",   InsuranceExpiry = new DateTime(2026, 1, 20) },
            new() { Plate = "UV90 WXY", Brand = "Renault",    Model = "Trafic",    InsuranceExpiry = new DateTime(2025, 9, 10) },
        };

        context.Vehicles.AddRange(vehicles);
        await context.SaveChangesAsync();

        var components = new List<VehicleComponent>
        {
            // Ford Transit
            new() { VehicleId = vehicles[0].Id, Name = "Engine",      SerialNumber = "ENG-00123", InstallationDate = new DateTime(2022, 4, 10) },
            new() { VehicleId = vehicles[0].Id, Name = "Gearbox",     SerialNumber = "GBX-00456", InstallationDate = new DateTime(2022, 4, 10) },
            new() { VehicleId = vehicles[0].Id, Name = "Front Tyres", SerialNumber = "TYR-00789", InstallationDate = new DateTime(2024, 1, 15) },

            // Vauxhall Vivaro
            new() { VehicleId = vehicles[1].Id, Name = "Engine",      SerialNumber = "ENG-00234", InstallationDate = new DateTime(2021, 6, 20) },
            new() { VehicleId = vehicles[1].Id, Name = "Brake Pads",  SerialNumber = "BRK-00567", InstallationDate = new DateTime(2023, 8, 5) },

            // Mercedes Sprinter
            new() { VehicleId = vehicles[2].Id, Name = "Engine",      SerialNumber = "ENG-00345", InstallationDate = new DateTime(2020, 3, 1) },
            new() { VehicleId = vehicles[2].Id, Name = "Alternator",  SerialNumber = "ALT-00678", InstallationDate = new DateTime(2023, 11, 22) },
            new() { VehicleId = vehicles[2].Id, Name = "Rear Tyres",  SerialNumber = "TYR-00890", InstallationDate = new DateTime(2024, 2, 18) },

            // Volkswagen Crafter
            new() { VehicleId = vehicles[3].Id, Name = "Engine",      SerialNumber = "ENG-00456", InstallationDate = new DateTime(2021, 9, 14) },
            new() { VehicleId = vehicles[3].Id, Name = "Clutch",      SerialNumber = "CLT-00123", InstallationDate = new DateTime(2023, 5, 30) },

            // Renault Trafic
            new() { VehicleId = vehicles[4].Id, Name = "Engine",      SerialNumber = "ENG-00567", InstallationDate = new DateTime(2019, 7, 8) },
            new() { VehicleId = vehicles[4].Id, Name = "Battery",     SerialNumber = "BAT-00234", InstallationDate = new DateTime(2023, 12, 3) },
        };

        context.VehicleComponents.AddRange(components);

        var assignments = new List<DriverAssignment>
        {
            // Ford Transit
            new() { VehicleId = vehicles[0].Id, DriverName = "James Whitfield",   StartDate = new DateTime(2022, 5, 1),  EndDate = new DateTime(2023, 4, 30) },
            new() { VehicleId = vehicles[0].Id, DriverName = "Oliver Hartley",    StartDate = new DateTime(2023, 5, 1),  EndDate = null },

            // Vauxhall Vivaro
            new() { VehicleId = vehicles[1].Id, DriverName = "Charlotte Simmons", StartDate = new DateTime(2021, 7, 1),  EndDate = new DateTime(2022, 12, 31) },
            new() { VehicleId = vehicles[1].Id, DriverName = "George Pemberton",  StartDate = new DateTime(2023, 1, 1),  EndDate = null },

            // Mercedes Sprinter
            new() { VehicleId = vehicles[2].Id, DriverName = "William Fletcher",  StartDate = new DateTime(2020, 4, 1),  EndDate = new DateTime(2021, 9, 15) },
            new() { VehicleId = vehicles[2].Id, DriverName = "Sophie Caldwell",   StartDate = new DateTime(2021, 9, 16), EndDate = new DateTime(2023, 6, 30) },
            new() { VehicleId = vehicles[2].Id, DriverName = "Harry Stanton",     StartDate = new DateTime(2023, 7, 1),  EndDate = null },

            // Volkswagen Crafter
            new() { VehicleId = vehicles[3].Id, DriverName = "Emily Thornton",    StartDate = new DateTime(2021, 10, 1), EndDate = new DateTime(2023, 3, 31) },
            new() { VehicleId = vehicles[3].Id, DriverName = "Thomas Kingsley",   StartDate = new DateTime(2023, 4, 1),  EndDate = null },

            // Renault Trafic
            new() { VehicleId = vehicles[4].Id, DriverName = "Isabella Marsh",    StartDate = new DateTime(2019, 8, 1),  EndDate = new DateTime(2022, 7, 31) },
            new() { VehicleId = vehicles[4].Id, DriverName = "Samuel Griffith",   StartDate = new DateTime(2022, 8, 1),  EndDate = null },
        };

        context.DriverAssignments.AddRange(assignments);
        await context.SaveChangesAsync();
    }
}