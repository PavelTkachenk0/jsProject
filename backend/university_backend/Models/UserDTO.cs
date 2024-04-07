using System.ComponentModel.DataAnnotations;

namespace university_backend.Models;

public class UserDTO
{
    [Required]
    public string Login { get; set; } = null!;
    
    public string? Password { get; set; } 
}

public class UpdateUserDTO
{
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}