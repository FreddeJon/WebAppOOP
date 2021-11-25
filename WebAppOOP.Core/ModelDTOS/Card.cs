using System.ComponentModel.DataAnnotations;

namespace WebAppOOP.Core.ModelDTOS
{
    public class Card
    {
        [Required]
        [Display(Name = "Name on card")]
        public string CCName { get; set; }

        [Required]
        [Display(Name = "Credit card number")]
        public string CCNumber { get; set; }

        [Required]
        [Display(Name = "Expiration")]
        public string CCExpiration { get; set; }

        [Required]
        [Display(Name = "CVV")]
        public string CCCvv { get; set; }
    }
}
