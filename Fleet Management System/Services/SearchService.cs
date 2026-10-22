using Fleet_Management_System.Data;
using Fleet_Management_System.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management_System.Services;

public class SearchService(ApplicationDbContext context) : ISearchService
{
    public async Task<IReadOnlyList<SearchResultDto>> SearchAsync(string query, int maxResults = 10)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
            return [];

        var term = query.Trim().ToLowerInvariant();
        var results = new List<SearchResultDto>();

        var vehicles = await context.Vehicles
            .AsNoTracking()
            .Where(v =>
                v.Plate.ToLower().Contains(term)
                || v.Brand.ToLower().Contains(term)
                || v.Model.ToLower().Contains(term))
            .OrderBy(v => v.Plate)
            .Take(maxResults)
            .Select(v => new SearchResultDto
            {
                Type = "Vehicle",
                Id = v.Id,
                Label = $"{v.Brand} {v.Model} ({v.Plate})",
                Subtitle = $"Insurance: {v.InsuranceExpiry:dd/MM/yyyy}",
                Url = $"/vehicles/{v.Id}"
            })
            .ToListAsync();

        results.AddRange(vehicles);

        var remaining = maxResults - results.Count;
        if (remaining > 0)
        {
            var drivers = await context.Drivers
                .AsNoTracking()
                .Where(d =>
                    d.Name.ToLower().Contains(term)
                    || (d.LicenseNumber != null && d.LicenseNumber.ToLower().Contains(term)))
                .OrderBy(d => d.Name)
                .Take(remaining)
                .Select(d => new SearchResultDto
                {
                    Type = "Driver",
                    Id = d.Id,
                    Label = d.Name,
                    Subtitle = d.LicenseNumber ?? "No license number",
                    Url = "/drivers"
                })
                .ToListAsync();

            results.AddRange(drivers);
        }

        return results;
    }
}
