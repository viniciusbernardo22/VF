using VF.Catalog.Domain.Entity.Base;
using VF.Catalog.Domain.Exceptions;

namespace VF.Catalog.Domain.Entity;

public class Category : Base.Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } 
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public Category(string name, string? description)
    {
        Name = name;
        Description = description;
        IsActive = true;
        
        ValidateEntity();
    }

    public Category(string name, string? description, bool isActive = true)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        
        ValidateEntity();
    }


    public sealed override void ValidateEntity()
    {
        if (String.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
        
        if(Description is null)
            throw new EntityValidationException($"{nameof(Description)} should not be null");
        
    }
}