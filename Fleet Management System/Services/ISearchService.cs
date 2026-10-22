using Fleet_Management_System.DTOs;

namespace Fleet_Management_System.Services;

public interface ISearchService
{
    Task<IReadOnlyList<SearchResultDto>> SearchAsync(string query, int maxResults = 10);
}
