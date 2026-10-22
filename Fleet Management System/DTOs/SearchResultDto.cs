namespace Fleet_Management_System.DTOs;

public class SearchResultDto
{
    public string Type { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
