using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public class UserForRegistrationDto
{
    //An init enforces immutability, so that once the object is initialized, it can't be changed.
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    [Required(ErrorMessage = "Username is required")]
    public string? UserName { get; init; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public ICollection<string>? Roles { get; init; }


}