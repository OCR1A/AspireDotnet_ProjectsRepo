using System.ComponentModel.DataAnnotations;

namespace IdentityManager.DTOs
{

    public class ConfirmEmailDto
    {

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Code { get; set; }

    }

}