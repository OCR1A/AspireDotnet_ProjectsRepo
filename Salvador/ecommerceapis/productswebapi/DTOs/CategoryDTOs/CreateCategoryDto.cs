using System.ComponentModel.DataAnnotations;

namespace ProductsWebApi.DTOs.CategoryDTOs
{

    public class CreateCategoryDto
    {

        [Required]
        [StringLength(60, MinimumLength = 4, ErrorMessage = "{0} must contain between {2} - {1} characters")]
        public string? Name { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 4, ErrorMessage = "{0} must contain between {2} - {1} characters")]
        public string? Description { get; set; }
        
        public int? FatherCategoryId { get; set; }

        public DateTime DateCreated { get; set; }

    }

}