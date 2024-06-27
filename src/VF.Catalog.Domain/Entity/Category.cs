namespace VF.Catalog.Domain.Entity;

public class Category
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; } 
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public Category(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public Category(string name, string description, bool isActive = true)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
    }
    
}