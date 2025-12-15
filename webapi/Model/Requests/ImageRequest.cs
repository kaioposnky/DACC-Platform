using System.ComponentModel.DataAnnotations;
using DaccApi.Model.Validation;

namespace DaccApi.Model.Requests
{
    public class ImageRequest
    {
        [ImageValidation]
        public IFormFile ImageFile { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(255)]
        public string ImageAlt { get; set; }
    }
}