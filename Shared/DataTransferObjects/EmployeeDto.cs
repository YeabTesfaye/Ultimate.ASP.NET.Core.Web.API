namespace Shared.DataTransferObjects;

public record EmployeeDto(){
    public Guid Id{ get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Position { get; set; }
}

