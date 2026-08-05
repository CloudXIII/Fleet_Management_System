using Fleet_Management_System.DTOs;
using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Vehicles.AnyAsync())
            return;

        var today = DateTime.UtcNow.Date;

        var vehicles = new List<Vehicle>
        {
            new() { Plate = "AB12 CDE", Brand = "Ford",       Model = "Transit",   InsuranceExpiry = today.AddMonths(7) },
            new() { Plate = "FG34 HIJ", Brand = "Vauxhall",   Model = "Vivaro",    InsuranceExpiry = today.AddDays(18) },
            new() { Plate = "KL56 MNO", Brand = "Mercedes",   Model = "Sprinter",  InsuranceExpiry = today.AddMonths(11) },
            new() { Plate = "PQ78 RST", Brand = "Volkswagen", Model = "Crafter",   InsuranceExpiry = today.AddDays(-12) },
            new() { Plate = "UV90 WXY", Brand = "Renault",    Model = "Trafic",    InsuranceExpiry = today.AddDays(5) },
            new() { Plate = "ZA11 BCD", Brand = "Ford",       Model = "Ranger",    InsuranceExpiry = today.AddMonths(4) },
            new() { Plate = "EF22 GHI", Brand = "Mercedes",   Model = "Vito",      InsuranceExpiry = today.AddMonths(2) },
            new() { Plate = "JK33 LMN", Brand = "Iveco",      Model = "Daily",     InsuranceExpiry = today.AddDays(45) },
        };

        context.Vehicles.AddRange(vehicles);
        await context.SaveChangesAsync();

        var components = new List<VehicleComponent>
        {
            new() { VehicleId = vehicles[0].Id, Name = "Engine",      SerialNumber = "ENG-00123", InstallationDate = new DateTime(2022, 4, 10) },
            new() { VehicleId = vehicles[0].Id, Name = "Gearbox",     SerialNumber = "GBX-00456", InstallationDate = new DateTime(2022, 4, 10) },
            new() { VehicleId = vehicles[0].Id, Name = "Front Tyres", SerialNumber = "TYR-00789", InstallationDate = new DateTime(2024, 1, 15) },
            new() { VehicleId = vehicles[1].Id, Name = "Engine",      SerialNumber = "ENG-00234", InstallationDate = new DateTime(2021, 6, 20) },
            new() { VehicleId = vehicles[1].Id, Name = "Brake Pads",  SerialNumber = "BRK-00567", InstallationDate = new DateTime(2023, 8, 5) },
            new() { VehicleId = vehicles[2].Id, Name = "Engine",      SerialNumber = "ENG-00345", InstallationDate = new DateTime(2020, 3, 1) },
            new() { VehicleId = vehicles[2].Id, Name = "Alternator",  SerialNumber = "ALT-00678", InstallationDate = new DateTime(2023, 11, 22) },
            new() { VehicleId = vehicles[2].Id, Name = "Rear Tyres",  SerialNumber = "TYR-00890", InstallationDate = new DateTime(2024, 2, 18) },
            new() { VehicleId = vehicles[3].Id, Name = "Engine",      SerialNumber = "ENG-00456", InstallationDate = new DateTime(2021, 9, 14) },
            new() { VehicleId = vehicles[3].Id, Name = "Clutch",      SerialNumber = "CLT-00123", InstallationDate = new DateTime(2023, 5, 30) },
            new() { VehicleId = vehicles[4].Id, Name = "Engine",      SerialNumber = "ENG-00567", InstallationDate = new DateTime(2019, 7, 8) },
            new() { VehicleId = vehicles[4].Id, Name = "Battery",     SerialNumber = "BAT-00234", InstallationDate = new DateTime(2023, 12, 3) },
            new() { VehicleId = vehicles[5].Id, Name = "Engine",      SerialNumber = "ENG-00678", InstallationDate = new DateTime(2023, 2, 12) },
            new() { VehicleId = vehicles[5].Id, Name = "Winch",       SerialNumber = "WNC-00321", InstallationDate = new DateTime(2023, 3, 1) },
            new() { VehicleId = vehicles[6].Id, Name = "Engine",      SerialNumber = "ENG-00789", InstallationDate = new DateTime(2022, 8, 19) },
            new() { VehicleId = vehicles[7].Id, Name = "Engine",      SerialNumber = "ENG-00890", InstallationDate = new DateTime(2021, 1, 7) },
            new() { VehicleId = vehicles[7].Id, Name = "Tail Lift",   SerialNumber = "TLF-00912", InstallationDate = new DateTime(2022, 6, 14) },
        };

        context.VehicleComponents.AddRange(components);

        var assignments = new List<DriverAssignment>
        {
            new() { VehicleId = vehicles[0].Id, DriverName = "James Whitfield",   StartDate = new DateTime(2022, 5, 1),  EndDate = new DateTime(2023, 4, 30) },
            new() { VehicleId = vehicles[0].Id, DriverName = "Oliver Hartley",    StartDate = new DateTime(2023, 5, 1),  EndDate = null },
            new() { VehicleId = vehicles[1].Id, DriverName = "Charlotte Simmons", StartDate = new DateTime(2021, 7, 1),  EndDate = new DateTime(2022, 12, 31) },
            new() { VehicleId = vehicles[1].Id, DriverName = "George Pemberton",  StartDate = new DateTime(2023, 1, 1),  EndDate = null },
            new() { VehicleId = vehicles[2].Id, DriverName = "William Fletcher",  StartDate = new DateTime(2020, 4, 1),  EndDate = new DateTime(2021, 9, 15) },
            new() { VehicleId = vehicles[2].Id, DriverName = "Sophie Caldwell",   StartDate = new DateTime(2021, 9, 16), EndDate = new DateTime(2023, 6, 30) },
            new() { VehicleId = vehicles[2].Id, DriverName = "Harry Stanton",     StartDate = new DateTime(2023, 7, 1),  EndDate = null },
            new() { VehicleId = vehicles[3].Id, DriverName = "Emily Thornton",    StartDate = new DateTime(2021, 10, 1), EndDate = new DateTime(2023, 3, 31) },
            new() { VehicleId = vehicles[3].Id, DriverName = "Thomas Kingsley",   StartDate = new DateTime(2023, 4, 1),  EndDate = null },
            new() { VehicleId = vehicles[4].Id, DriverName = "Isabella Marsh",    StartDate = new DateTime(2019, 8, 1),  EndDate = new DateTime(2022, 7, 31) },
            new() { VehicleId = vehicles[4].Id, DriverName = "Samuel Griffith",   StartDate = new DateTime(2022, 8, 1),  EndDate = null },
            new() { VehicleId = vehicles[5].Id, DriverName = "Nora Bennett",      StartDate = new DateTime(2023, 3, 15), EndDate = null },
            new() { VehicleId = vehicles[6].Id, DriverName = "Lucas Reed",        StartDate = new DateTime(2022, 9, 1),  EndDate = null },
            new() { VehicleId = vehicles[7].Id, DriverName = "Amelia Clarke",     StartDate = new DateTime(2021, 2, 1),  EndDate = new DateTime(2024, 1, 31) },
            new() { VehicleId = vehicles[7].Id, DriverName = "Ethan Brooks",      StartDate = new DateTime(2024, 2, 1),  EndDate = null },
        };

        context.DriverAssignments.AddRange(assignments);
        await context.SaveChangesAsync();
    }
}
