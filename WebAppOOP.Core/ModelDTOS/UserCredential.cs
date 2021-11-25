using System.ComponentModel.DataAnnotations;

namespace WebAppOOP.Core.ModelDTOS
{
    public class UserCredential
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
