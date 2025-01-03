using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public abstract class BaseEntity
{
    public BaseEntity()
    {
    }

    public BaseEntity(
        long id)
    {
        Id = id;
    }

    [Key]
    public long Id { get; set; }
}