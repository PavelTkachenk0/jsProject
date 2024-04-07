using Newtonsoft.Json;

namespace university_backend.Models;

public class ContactMessageDTO : UpdateMessageDTO
{ 
    public string Email { get; set; } = null!;
}

public class UpdateMessageDTO 
{
    public string Message { set; get; } = null!;
}