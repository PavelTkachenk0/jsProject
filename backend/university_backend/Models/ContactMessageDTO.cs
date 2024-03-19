using Newtonsoft.Json;

namespace university_backend.Models;

public class ContactMessageDTO
{
    public string Email { get; set; } = null!;
    public string Message { get; set; } = null!;
}