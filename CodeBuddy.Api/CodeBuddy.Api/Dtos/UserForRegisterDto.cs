using System.ComponentModel.DataAnnotations;

namespace CodeBuddy.Api.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(maximumLength:15, MinimumLength = 5, ErrorMessage = "Please enter valid password")]
        public string Password { get; set; }
    }
}
