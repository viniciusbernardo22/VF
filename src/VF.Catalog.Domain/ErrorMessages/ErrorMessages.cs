namespace VF.Catalog.Domain;

public class ErrorMessages
{
    public string EmptyOrNullMessage(string property)
        => string.Format($"{property} should not be empty or null");
    
    public string MinLengthMessage(string property, int length)
        => string.Format($"{property} should have a minimum of {length} characters long");
    
    public string MaxLengthMessage(string property, int length)
        => string.Format($"{property} should have a maximum of {length} characters long");

    public string ShouldNotBeNullMessage(string property)
        => string.Format($"{property} should not be null");
    
}