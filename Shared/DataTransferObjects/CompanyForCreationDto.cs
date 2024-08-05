using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record CompanyForCreationDto{
    [Required(ErrorMessage ="Company Name is required")]
    [MaxLength(20,ErrorMessage ="Maximum length for the company Name is 30 character")]
    public string? Name { get; set; }
    [Required(ErrorMessage ="Address is required")]
     public string? Address { get; set; }
     [Required(ErrorMessage ="Country Name is required")]
     public string? Country { get; set; } 
    public required IEnumerable<EmployeeForCreationDto> Employees {get; set; }
}