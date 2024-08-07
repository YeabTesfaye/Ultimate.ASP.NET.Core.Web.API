namespace Shared.RequestFeatures;

public class EmployeeParameters : RequestParameters
{
    //We want to sort our results by name
    public EmployeeParameters() => OrderBy = "name";
    public uint MinAge { get; set; }
    public uint MaxAge { get; set; } = int.MaxValue;
    public bool ValidAgeRange  => MaxAge > MinAge;
    public string? SearchTerm { get; set; }
}