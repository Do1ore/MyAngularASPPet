using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySuperApi.Models
{
    public class ProfileImageClaims
    {
        [Required]
        [ForeignKey("AppUser")]
        public Guid UserId { get; set; }
        public AppUser? User { get; set; }

        [Required]
        [ForeignKey("UserProfileImage")]
        public Guid ProfileImageId { get; set; }
        public UserProfileImage? ProfileImage { get; set; }
    }
}
