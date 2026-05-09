namespace MediaStore.Api.Domain;

public record PagedResult<T>(
    IReadOnlyCollection<T> Items,
    int TotalCount);