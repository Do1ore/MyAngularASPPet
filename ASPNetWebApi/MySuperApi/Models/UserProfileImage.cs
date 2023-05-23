using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySuperApi.Models
{
    public class UserProfileImage
    {
        [Key]
        public Guid ImageId { get; set; }
        public AppUser? AppUser { get; set; }
        [ForeignKey("AppUser")]
        public Guid AppUserId { get; set; }
        public string? FileName { get; set; }
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? FormImage { get; set; }
    }
}
