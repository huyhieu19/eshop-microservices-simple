namespace Ordering.Domain.Abstractions;

public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}

public interface IEntity
{
    public DateTime? CreateAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string? CreateBy { get; set; }
    public string? LastModifiedBy { get; set; }
}
