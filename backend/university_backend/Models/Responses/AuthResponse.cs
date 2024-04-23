namespace university_backend.Models.Responses;

public class AuthResponse : UserDTO
{
    public string[] Roles { get; set; } = null!;
}
