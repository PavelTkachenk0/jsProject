using System.ComponentModel.DataAnnotations;

namespace university_backend.Models;

public class UserDTO
{
    [Required]
    public string Login { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
