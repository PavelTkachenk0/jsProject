using System.ComponentModel.DataAnnotations;

namespace university_backend.Models;

public class UserDTO
{
    [Required]
    public string Login { get; set; } = null!;
    
    public string? Password { get; set; } 
}
