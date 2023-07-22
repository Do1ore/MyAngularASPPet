using Microsoft.Build.Framework;

namespace MySuperApi.Api.DTOs
{
    public class UserDto
    {
        [Required]
        public string? email { get; set; }
        [Required]
        public string? password { get; set; }

    }
}
