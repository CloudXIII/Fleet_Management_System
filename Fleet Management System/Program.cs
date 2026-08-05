using System.Text.Json.Serialization;
using Fleet_Management_System.Data;
using Fleet_Management_System.Profiles;
using Fleet_Management_System.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleComponentService, VehicleComponentService>();
builder.Services.AddScoped<IDriverAssignmentService, DriverAssignmentService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddAutoMapper(cfg => { },
    typeof(VehicleProfile),
    typeof(VehicleComponentProfile),
    typeof(DriverAssignmentProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevClient", policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevClient");
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DatabaseSeeder.SeedAsync(context);
}

app.Run();

public partial class Program;
