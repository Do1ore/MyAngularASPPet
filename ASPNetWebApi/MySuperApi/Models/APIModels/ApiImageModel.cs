using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySuperApi.Models.APIModels
{
    public class ApiImageModel
    {
        [Key]
        public Guid ImageId { get; set; }
        public string? FileName { get; set; }
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? FormImage { get; set; }
    }
}
