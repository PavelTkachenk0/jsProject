namespace university_backend.Models.Requests;

public class UserRequest : UserDTO
{
    public string Role { get; set; } = null!;
}
