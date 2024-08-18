namespace BuildingBlocks;

public static class PagingIQueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than 0.");
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");

        int skip = (pageNumber - 1) * pageSize;
        return source.Skip(skip).Take(pageSize);
    }
}
