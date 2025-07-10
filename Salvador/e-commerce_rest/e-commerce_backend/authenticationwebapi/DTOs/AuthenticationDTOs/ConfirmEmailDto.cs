using System.ComponentModel.DataAnnotations;

namespace IdentityManager.DTOs
{

    public class ConfirmEmailDto
    {

        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? ConfirmEmailToken { get; set; }

    }

}