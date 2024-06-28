using VF.Catalog.Domain;
using VF.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = VF.Catalog.Domain.Entity;
namespace VF.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    private readonly ErrorMessages _errorMessages = new ErrorMessages();
    
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        DateTime datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        DateTime datetimeAfter = DateTime.Now;
        
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
        
    }

    
    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = new 
        {
            Name = "category name",
            Description = "category description",
            IsActive = isActive
        };

        DateTime datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        DateTime datetimeAfter = DateTime.Now;
        
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore );
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(category.IsActive, isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void InstantiateErrorWhenNameIsEmpty(string name)
    {
        Action action = () => new DomainEntity.Category(name, "description");

        var exception = Assert.Throws<EntityValidationException>(action);

        var expectedMessage = _errorMessages.EmptyOrNullMessage("Name");
        Assert.Equal(expectedMessage, exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("category name", null);
        var exception = Assert.Throws<EntityValidationException>(action);
        var expectedMessage = _errorMessages.ShouldNotBeNullMessage("Description");
        
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsSmallerThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("c")]
    [InlineData("ca")]
    public void InstantiateErrorWhenNameIsSmallerThan3Characters(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "valid description");
        
        var exception = Assert.Throws<EntityValidationException>(action);
        var expectedMessage = _errorMessages.MinLengthMessage("Name", 3);
        Assert.Equal(expectedMessage, exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        
        Action action = () => new DomainEntity.Category(invalidName, "valid description");
        
        var exception = Assert.Throws<EntityValidationException>(action);
        var expectedMessage = _errorMessages.MaxLengthMessage("Name", 255);
        Assert.Equal(expectedMessage, exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenCategoryIsGreaterThan10KCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenCategoryIsGreaterThan10KCharacters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(0, 10001).Select(_ => "d").ToArray());
        
        Action action = () => new DomainEntity.Category("Valid Name", invalidDescription);
        
        var exception = Assert.Throws<EntityValidationException>(action);
        var expectedMessage = _errorMessages.MaxLengthMessage("Description", 10000);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact(DisplayName = nameof(ActivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void ActivateCategory()
    {
        var validData = new 
        {
            Name = "category name",
            Description = "category description",
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, false);

        category.Activate();
        
        Assert.True(category.IsActive);
    }
    
    [Fact(DisplayName = nameof(DeactivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void DeactivateCategory()
    {
        var validData = new 
        {
            Name = "category name",
            Description = "category description",
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);

        category.Deactivate();
        
        Assert.False(category.IsActive);
    }


    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategory()
    {
        var category = new DomainEntity.Category("CategoryName", "Category Description");
        var newValues = new { Name = "New Category Name", Description = "New Description Name" };

        category.Update(newValues.Name, newValues.Description);
        
        Assert.Equal(category.Name, newValues.Name);
        Assert.Equal(category.Description, newValues.Description);
    }
    
    [Fact(DisplayName = nameof(UpdateOnlyCategoryName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyCategoryName()
    {
        var category = new DomainEntity.Category("CategoryName", "Category Description");
        var newValues = new { Name = "New Category Name"};
        var currentDescription = category.Description;
        
        category.Update(newValues.Name);
        
        Assert.Equal(category.Name, newValues.Name);
        Assert.Equal(currentDescription, category.Description);
    }
    
    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    public void UpdateErrorWhenNameIsEmpty(string name)
    {
        var category = new DomainEntity.Category("CategoryName", "Category Description");
        Action action = () => category.Update(name!);

        var exception = Assert.Throws<EntityValidationException>(action);

        var expectedMessage = _errorMessages.EmptyOrNullMessage("Name");
        Assert.Equal(expectedMessage, exception.Message);
    }
    
    
    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsSmallerThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("23")]
    [InlineData("c")]
    [InlineData("ca")]
    public void UpdateErrorWhenNameIsSmallerThan3Characters(string invalidName)
    {
        var category = new DomainEntity.Category("CategoryName", "Category Description");
        Action action = () => category.Update(invalidName);
        
        var exception = Assert.Throws<EntityValidationException>(action);
        var expectedMessage = _errorMessages.MinLengthMessage("Name", 3);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = new DomainEntity.Category("CategoryName", "Category Description");
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());

        Action action = () => category.Update(invalidName);
        
        var exception = Assert.Throws<EntityValidationException>(action);
        var expectedMessage = _errorMessages.MaxLengthMessage("Name", 255);
        Assert.Equal(expectedMessage, exception.Message);
    }
    
    [Fact(DisplayName = nameof(UpdateErrorWhenCategoryIsGreaterThan10KCharacters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenCategoryIsGreaterThan10KCharacters()
    {
        var category = new DomainEntity.Category("CategoryName", "Category Description");
        var invalidDescription = String.Join(null, Enumerable.Range(0, 10001).Select(_ => "d").ToArray());

        Action action = () => category.Update("validName", invalidDescription);
        
        var exception = Assert.Throws<EntityValidationException>(action);
        var expectedMessage = _errorMessages.MaxLengthMessage("Description", 10000);
        Assert.Equal(expectedMessage, exception.Message);
    }
}