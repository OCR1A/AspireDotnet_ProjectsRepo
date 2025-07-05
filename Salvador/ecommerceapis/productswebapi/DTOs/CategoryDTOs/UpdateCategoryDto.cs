using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ProductsWebApi.DTOs.CategoryDTOs
{

    public class UpdateCategoryDto
    {
        [AllowNull]
        [StringLength(60, MinimumLength = 4, ErrorMessage = "{0} must contain between {2} - {1} characters")]
        public string? Name { get; set; }

        [AllowNull]
        [StringLength(255, MinimumLength = 4, ErrorMessage = "{0} must contain between {2} - {1} characters")]
        public string? Description { get; set; }

        [AllowNull]
        public int? FatherCategoryId { get; set; }

    }
    
}