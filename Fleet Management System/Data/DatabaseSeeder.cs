using Fleet_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Vehicles.AnyAsync())
            return;

        var today = DateTime.Today;

        var drivers = new List<Driver>
        {
            new() { Name = "James Whitfield",    LicenseNumber = "WHIT123456", Phone = "07700 900111" },
            new() { Name = "Oliver Hartley",     LicenseNumber = "HART234567", Phone = "07700 900222" },
            new() { Name = "Charlotte Simmons",  LicenseNumber = "SIMM345678", Phone = "07700 900333" },
            new() { Name = "George Pemberton",   LicenseNumber = "PEMB456789", Phone = "07700 900444" },
            new() { Name = "William Fletcher",   LicenseNumber = "FLET567890", Phone = "07700 900555" },
            new() { Name = "Sophie Caldwell",    LicenseNumber = "CALD678901", Phone = "07700 900666" },
            new() { Name = "Harry Stanton",      LicenseNumber = "STAN789012", Phone = "07700 900777" },
            new() { Name = "Emily Thornton",     LicenseNumber = "THOR890123", Phone = "07700 900888" },
            new() { Name = "Thomas Kingsley",    LicenseNumber = "KING901234", Phone = "07700 900999" },
            new() { Name = "Isabella Marsh",     LicenseNumber = "MARS012345", Phone = "07700 901000" },
            new() { Name = "Samuel Griffith",    LicenseNumber = "GRIF112233", Phone = "07700 901111" },
            new() { Name = "Amelia Hughes",      LicenseNumber = "HUGH223344", Phone = "07700 901222" },
            new() { Name = "Benjamin Clarke",    LicenseNumber = "CLAR334455", Phone = "07700 901333" },
            new() { Name = "Olivia Bennett",     LicenseNumber = "BENN445566", Phone = "07700 901444" },
            new() { Name = "Jack Morrison",      LicenseNumber = "MORR556677", Phone = "07700 901555" },
            new() { Name = "Grace Patel",        LicenseNumber = "PATE667788", Phone = "07700 901666" },
            new() { Name = "Noah Sullivan",      LicenseNumber = "SULL778899", Phone = "07700 901777" },
            new() { Name = "Mia Richardson",     LicenseNumber = "RICH889900", Phone = "07700 901888" },
            new() { Name = "Lucas Ahmed",        LicenseNumber = "AHME990011", Phone = "07700 901999" },
            new() { Name = "Ella Fitzgerald",    LicenseNumber = "FITZ001122", Phone = "07700 902000" },
        };

        context.Drivers.AddRange(drivers);
        await context.SaveChangesAsync();

        var vehicles = new List<Vehicle>
        {
            // Healthy insurance
            new() { Plate = "AB12 CDE", Brand = "Ford",       Model = "Transit",    InsuranceExpiry = today.AddMonths(8) },
            new() { Plate = "FG34 HIJ", Brand = "Vauxhall",   Model = "Vivaro",     InsuranceExpiry = today.AddMonths(4) },
            new() { Plate = "KL56 MNO", Brand = "Mercedes",   Model = "Sprinter",   InsuranceExpiry = today.AddYears(1) },
            new() { Plate = "PQ78 RST", Brand = "Volkswagen", Model = "Crafter",    InsuranceExpiry = today.AddMonths(10) },
            new() { Plate = "CD11 FGH", Brand = "Toyota",     Model = "Proace",     InsuranceExpiry = today.AddMonths(5) },
            new() { Plate = "EF22 JKL", Brand = "Peugeot",    Model = "Boxer",      InsuranceExpiry = today.AddMonths(7) },
            new() { Plate = "GH33 MNP", Brand = "Iveco",      Model = "Daily",      InsuranceExpiry = today.AddMonths(9) },
            new() { Plate = "JK44 QRS", Brand = "Nissan",     Model = "NV300",      InsuranceExpiry = today.AddMonths(3) },

            // Expiring within 30 days (dashboard alerts)
            new() { Plate = "UV90 WXY", Brand = "Renault",    Model = "Trafic",     InsuranceExpiry = today.AddDays(12) },
            new() { Plate = "LM55 TUV", Brand = "Fiat",       Model = "Ducato",     InsuranceExpiry = today.AddDays(22) },
            new() { Plate = "NO66 WXY", Brand = "Citroen",    Model = "Relay",      InsuranceExpiry = today.AddDays(5) },

            // Already expired
            new() { Plate = "RS77 ZAB", Brand = "Ford",       Model = "Custom",     InsuranceExpiry = today.AddDays(-8) },
            new() { Plate = "TU88 CDE", Brand = "Mercedes",   Model = "Vito",       InsuranceExpiry = today.AddDays(-20) },

            // Unassigned / spare vehicles (no active driver)
            new() { Plate = "VW99 FGH", Brand = "Volkswagen", Model = "Transporter", InsuranceExpiry = today.AddMonths(6) },
            new() { Plate = "XY10 IJK", Brand = "Ford",       Model = "Ranger",     InsuranceExpiry = today.AddMonths(2) },
        };

        context.Vehicles.AddRange(vehicles);
        await context.SaveChangesAsync();

        var components = new List<VehicleComponent>
        {
            // Ford Transit
            Comp(vehicles[0], "Engine", "ENG-10001", today.AddYears(-4)),
            Comp(vehicles[0], "Gearbox", "GBX-10002", today.AddYears(-4)),
            Comp(vehicles[0], "Front Tyres", "TYR-10003", today.AddMonths(-10)),
            Comp(vehicles[0], "Battery", "BAT-10004", today.AddMonths(-14)),
            Comp(vehicles[0], "GPS Tracker", "GPS-10005", today.AddYears(-2)),

            // Vauxhall Vivaro
            Comp(vehicles[1], "Engine", "ENG-11001", today.AddYears(-5)),
            Comp(vehicles[1], "Brake Pads", "BRK-11002", today.AddMonths(-8)),
            Comp(vehicles[1], "Alternator", "ALT-11003", today.AddYears(-1)),
            Comp(vehicles[1], "Rear Tyres", "TYR-11004", today.AddMonths(-6)),

            // Mercedes Sprinter
            Comp(vehicles[2], "Engine", "ENG-12001", today.AddYears(-6)),
            Comp(vehicles[2], "Alternator", "ALT-12002", today.AddYears(-2)),
            Comp(vehicles[2], "Rear Tyres", "TYR-12003", today.AddMonths(-9)),
            Comp(vehicles[2], "Clutch", "CLT-12004", today.AddYears(-1)),
            Comp(vehicles[2], "Dashboard Camera", "CAM-12005", today.AddMonths(-18)),

            // Volkswagen Crafter
            Comp(vehicles[3], "Engine", "ENG-13001", today.AddYears(-4)),
            Comp(vehicles[3], "Clutch", "CLT-13002", today.AddYears(-2)),
            Comp(vehicles[3], "Battery", "BAT-13003", today.AddMonths(-11)),
            Comp(vehicles[3], "Front Tyres", "TYR-13004", today.AddMonths(-4)),

            // Toyota Proace
            Comp(vehicles[4], "Engine", "ENG-14001", today.AddYears(-3)),
            Comp(vehicles[4], "Gearbox", "GBX-14002", today.AddYears(-3)),
            Comp(vehicles[4], "Brake Discs", "BRK-14003", today.AddMonths(-7)),

            // Peugeot Boxer
            Comp(vehicles[5], "Engine", "ENG-15001", today.AddYears(-5)),
            Comp(vehicles[5], "Battery", "BAT-15002", today.AddMonths(-16)),
            Comp(vehicles[5], "Side Door Motor", "SDR-15003", today.AddYears(-1)),
            Comp(vehicles[5], "GPS Tracker", "GPS-15004", today.AddMonths(-20)),

            // Iveco Daily
            Comp(vehicles[6], "Engine", "ENG-16001", today.AddYears(-7)),
            Comp(vehicles[6], "Turbocharger", "TRB-16002", today.AddYears(-2)),
            Comp(vehicles[6], "Rear Tyres", "TYR-16003", today.AddMonths(-5)),
            Comp(vehicles[6], "Alternator", "ALT-16004", today.AddMonths(-13)),

            // Nissan NV300
            Comp(vehicles[7], "Engine", "ENG-17001", today.AddYears(-3)),
            Comp(vehicles[7], "Front Tyres", "TYR-17002", today.AddMonths(-3)),
            Comp(vehicles[7], "Battery", "BAT-17003", today.AddMonths(-9)),

            // Renault Trafic
            Comp(vehicles[8], "Engine", "ENG-18001", today.AddYears(-6)),
            Comp(vehicles[8], "Battery", "BAT-18002", today.AddYears(-1)),
            Comp(vehicles[8], "Brake Pads", "BRK-18003", today.AddMonths(-12)),
            Comp(vehicles[8], "Radio Unit", "RAD-18004", today.AddYears(-4)),

            // Fiat Ducato
            Comp(vehicles[9], "Engine", "ENG-19001", today.AddYears(-4)),
            Comp(vehicles[9], "Gearbox", "GBX-19002", today.AddYears(-4)),
            Comp(vehicles[9], "Clutch", "CLT-19003", today.AddMonths(-15)),
            Comp(vehicles[9], "GPS Tracker", "GPS-19004", today.AddMonths(-8)),

            // Citroen Relay
            Comp(vehicles[10], "Engine", "ENG-20001", today.AddYears(-5)),
            Comp(vehicles[10], "Front Tyres", "TYR-20002", today.AddMonths(-2)),
            Comp(vehicles[10], "Alternator", "ALT-20003", today.AddYears(-1)),

            // Ford Custom (expired insurance)
            Comp(vehicles[11], "Engine", "ENG-21001", today.AddYears(-3)),
            Comp(vehicles[11], "Battery", "BAT-21002", today.AddMonths(-6)),
            Comp(vehicles[11], "Rear Tyres", "TYR-21003", today.AddMonths(-11)),

            // Mercedes Vito (expired insurance)
            Comp(vehicles[12], "Engine", "ENG-22001", today.AddYears(-8)),
            Comp(vehicles[12], "Gearbox", "GBX-22002", today.AddYears(-8)),
            Comp(vehicles[12], "Brake Pads", "BRK-22003", today.AddMonths(-14)),
            Comp(vehicles[12], "Dashboard Camera", "CAM-22004", today.AddYears(-2)),

            // VW Transporter (spare)
            Comp(vehicles[13], "Engine", "ENG-23001", today.AddYears(-2)),
            Comp(vehicles[13], "Battery", "BAT-23002", today.AddMonths(-4)),
            Comp(vehicles[13], "Front Tyres", "TYR-23003", today.AddMonths(-1)),

            // Ford Ranger (spare)
            Comp(vehicles[14], "Engine", "ENG-24001", today.AddYears(-1)),
            Comp(vehicles[14], "Gearbox", "GBX-24002", today.AddYears(-1)),
            Comp(vehicles[14], "Spare Wheel", "TYR-24003", today.AddMonths(-3)),
            Comp(vehicles[14], "Tow Hitch", "TOW-24004", today.AddMonths(-10)),
        };

        context.VehicleComponents.AddRange(components);

        var assignments = new List<DriverAssignment>
        {
            // Transit — long history, currently Oliver
            Assign(vehicles[0], drivers[0], today.AddYears(-4), today.AddYears(-3).AddMonths(-1)),
            Assign(vehicles[0], drivers[11], today.AddYears(-3), today.AddYears(-2)),
            Assign(vehicles[0], drivers[1], today.AddYears(-2).AddDays(1), null),

            // Vivaro
            Assign(vehicles[1], drivers[2], today.AddYears(-4), today.AddYears(-2).AddMonths(-6)),
            Assign(vehicles[1], drivers[3], today.AddYears(-2).AddMonths(-5), null),

            // Sprinter — three past + active
            Assign(vehicles[2], drivers[4], today.AddYears(-5), today.AddYears(-4)),
            Assign(vehicles[2], drivers[5], today.AddYears(-4).AddDays(1), today.AddYears(-2)),
            Assign(vehicles[2], drivers[12], today.AddYears(-2).AddDays(1), today.AddMonths(-8)),
            Assign(vehicles[2], drivers[6], today.AddMonths(-7), null),

            // Crafter
            Assign(vehicles[3], drivers[7], today.AddYears(-3), today.AddYears(-1)),
            Assign(vehicles[3], drivers[8], today.AddYears(-1).AddDays(1), null),

            // Proace
            Assign(vehicles[4], drivers[13], today.AddYears(-2), today.AddMonths(-10)),
            Assign(vehicles[4], drivers[9], today.AddMonths(-9), null),

            // Boxer
            Assign(vehicles[5], drivers[14], today.AddYears(-3), today.AddYears(-1).AddMonths(-2)),
            Assign(vehicles[5], drivers[10], today.AddYears(-1), null),

            // Daily
            Assign(vehicles[6], drivers[15], today.AddYears(-4), today.AddYears(-2)),
            Assign(vehicles[6], drivers[16], today.AddYears(-2).AddDays(1), null),

            // NV300
            Assign(vehicles[7], drivers[17], today.AddYears(-2), today.AddMonths(-4)),
            Assign(vehicles[7], drivers[18], today.AddMonths(-3), null),

            // Trafic (insurance soon)
            Assign(vehicles[8], drivers[9], today.AddYears(-5), today.AddYears(-3)),
            Assign(vehicles[8], drivers[19], today.AddYears(-3).AddDays(1), today.AddMonths(-6)),
            Assign(vehicles[8], drivers[0], today.AddMonths(-5), null),

            // Ducato (insurance soon)
            Assign(vehicles[9], drivers[1], today.AddYears(-3), today.AddYears(-1)),
            Assign(vehicles[9], drivers[11], today.AddYears(-1).AddDays(1), null),

            // Relay (insurance soon)
            Assign(vehicles[10], drivers[2], today.AddYears(-2), today.AddMonths(-2)),
            Assign(vehicles[10], drivers[12], today.AddMonths(-1), null),

            // Custom (expired insurance) — still active
            Assign(vehicles[11], drivers[3], today.AddYears(-2), today.AddMonths(-8)),
            Assign(vehicles[11], drivers[13], today.AddMonths(-7), null),

            // Vito (expired insurance) — ended, currently unassigned
            Assign(vehicles[12], drivers[4], today.AddYears(-4), today.AddYears(-2)),
            Assign(vehicles[12], drivers[14], today.AddYears(-2).AddDays(1), today.AddMonths(-1)),

            // Transporter & Ranger — history only, currently spare
            Assign(vehicles[13], drivers[5], today.AddYears(-1), today.AddMonths(-3)),
            Assign(vehicles[14], drivers[6], today.AddMonths(-10), today.AddMonths(-2)),
        };

        context.DriverAssignments.AddRange(assignments);
        await context.SaveChangesAsync();
    }

    private static VehicleComponent Comp(Vehicle vehicle, string name, string serial, DateTime installed) =>
        new()
        {
            VehicleId = vehicle.Id,
            Name = name,
            SerialNumber = serial,
            InstallationDate = installed
        };

    private static DriverAssignment Assign(Vehicle vehicle, Driver driver, DateTime start, DateTime? end) =>
        new()
        {
            VehicleId = vehicle.Id,
            DriverId = driver.Id,
            StartDate = start,
            EndDate = end
        };
}
