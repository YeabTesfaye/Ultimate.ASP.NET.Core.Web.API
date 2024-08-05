using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public abstract record EmployeeForManipulationDto
{
    [Required(ErrorMessage ="Employee name is required filed.")]
    [MaxLength(30,ErrorMessage ="Maximum length for the Name is 30 characters")]
    public string? Name { get; set; }
    [Range(18, int.MaxValue,ErrorMessage ="Age is required and it can't be lower than 18")]
    public int Age { get; set; }
    [Required(ErrorMessage ="Position is required field.")]
    [MaxLength(20,ErrorMessage ="Maximum length for the Position is 20 characters.")]
    public string? Position { get; set; }
}