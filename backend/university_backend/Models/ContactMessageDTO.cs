using Newtonsoft.Json;

namespace university_backend.Models;

public class ContactMessageDTO
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("email")]
    public string Email { get; set; } = null!;

    [JsonProperty("message")]
    public string Message { get; set; } = null!;
}