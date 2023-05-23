using Microsoft.Build.Framework;

namespace MySuperApi.DTOs
{
    public class UserDto
    {
        [Required]
        public string? email { get; set; }
        [Required]
        public string? password { get; set; }

    }
}
