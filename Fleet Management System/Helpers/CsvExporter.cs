using System.Globalization;
using System.Text;
using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Helpers;

public static class CsvExporter
{
    public static string Vehicles(IEnumerable<VehicleReadDto> vehicles)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,Plate,Brand,Model,InsuranceExpiry,HasActiveDriver,IsInsuranceExpiringSoon");

        foreach (var v in vehicles)
        {
            sb.Append(v.Id).Append(',')
              .Append(Escape(v.Plate)).Append(',')
              .Append(Escape(v.Brand)).Append(',')
              .Append(Escape(v.Model)).Append(',')
              .Append(v.InsuranceExpiry.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Append(',')
              .Append(v.HasActiveDriver).Append(',')
              .Append(v.IsInsuranceExpiringSoon)
              .AppendLine();
        }

        return sb.ToString();
    }

    public static string Assignments(IEnumerable<DriverAssignmentReadDto> assignments)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,DriverId,DriverName,StartDate,EndDate,IsActive,VehicleId");

        foreach (var a in assignments)
        {
            sb.Append(a.Id).Append(',')
              .Append(a.DriverId).Append(',')
              .Append(Escape(a.DriverName)).Append(',')
              .Append(a.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Append(',')
              .Append(a.EndDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "").Append(',')
              .Append(a.IsActive).Append(',')
              .Append(a.VehicleId)
              .AppendLine();
        }

        return sb.ToString();
    }

    private static string Escape(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";

        return value;
    }
}
