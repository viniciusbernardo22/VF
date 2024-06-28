
using VF.Catalog.Domain.Exceptions;
namespace VF.Catalog.Domain.Entity;

public class Category : Entity
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
        ErrorMessages errorMessage = new ErrorMessages();
        
        if (String.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException(errorMessage.EmptyOrNullMessage(nameof(Name)));
        
        if(Name.Length < 3)
            throw new EntityValidationException(errorMessage.MinLengthMessage(nameof(Name), 3));
        
        if(Name.Length > 255)
            throw new EntityValidationException(errorMessage.MaxLengthMessage(nameof(Name), 255));
        
        if(Description is null)
            throw new EntityValidationException(errorMessage.ShouldNotBeNullMessage(nameof(Description)));
        
        if(Description.Length > 10000)
            throw new EntityValidationException(errorMessage.MaxLengthMessage(nameof(Description), 10000));
        
    }
}