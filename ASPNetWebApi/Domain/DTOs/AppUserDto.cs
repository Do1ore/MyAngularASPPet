namespace Domain.DTOs;

/// <summary>
/// Dto contains information that can be seen by other chat users
/// </summary>
public class AppUserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime AccountCreated { get; set; }
    public DateTime AccountLastTimeEdited { get; set; }
    public DateTime LastTimeOnline { get; set; }
    public byte[]? CurrentImageBytes { get; set; }
}