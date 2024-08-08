using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public class UserForAuthenticationDto
{
    [Required(ErrorMessage = "User name is required.")]
    public string? Username { get; init; }
    [Required(ErrorMessage = "Password name is required.")]
    public string? Password { get; init; }
}