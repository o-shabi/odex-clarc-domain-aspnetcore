namespace Odex.AspNetCore.Clarc.Domain.ValueObjects.Responses;

/// <summary>
/// Standard paged payload: a window of items plus the total count across all pages.
/// </summary>
/// <typeparam name="T">Element type in the current page.</typeparam>
/// <param name="Data">Items for the current page.</param>
/// <param name="Total">Total number of items matching the query (not just the page size).</param>
public record PagedResponse<T>(List<T> Data, int Total) : BaseResponse;
