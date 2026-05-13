namespace Odex.AspNetCore.Clarc.Domain.ValueObjects.Requests;

/// <summary>
/// Common pagination and sorting inputs for list endpoints. Derive a concrete record when you need additional filters.
/// </summary>
public abstract record PagedRequest : BaseRequest
{
    #region Pagination

    private readonly int _page = 1;
    private readonly int _pageSize = 20;

    /// <summary>
    /// Gets the one-based page index (minimum 1 after initialization).
    /// </summary>
    public int Page
    {
        get => _page;
        init => _page = Math.Max(1, value);
    }

    /// <summary>
    /// Gets the page size (clamped between 1 and 512 inclusive after initialization).
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = Math.Clamp(value, 1, 512);
    }

    /// <summary>
    /// Gets the number of rows to skip for data access layers (<c>(Page - 1) * PageSize</c>).
    /// </summary>
    public int SkipCount => (Page - 1) * PageSize;

    #endregion

    #region Sorting

    /// <summary>
    /// Gets the optional property or column name to sort by (interpretation is application-specific).
    /// </summary>
    public string? SortBy { get; init; }

    /// <summary>
    /// Gets a value indicating whether sorting is descending.
    /// </summary>
    public bool SortDescending { get; init; } = false;

    #endregion
}
