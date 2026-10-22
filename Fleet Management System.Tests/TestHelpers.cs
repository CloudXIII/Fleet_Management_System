using AutoMapper;
using Fleet_Management_System.Data;
using Fleet_Management_System.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fleet_Management_System.Tests;

public static class TestHelpers
{
    public static ApplicationDbContext CreateContext(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public static IMapper CreateMapper()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => { },
            typeof(VehicleProfile),
            typeof(VehicleComponentProfile),
            typeof(DriverAssignmentProfile),
            typeof(DriverProfile));

        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }
}
