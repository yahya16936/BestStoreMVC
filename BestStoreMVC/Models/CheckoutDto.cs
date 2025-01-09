using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models
{
    public class CheckoutDto
    {
        [Required(ErrorMessage = "Delivery adress is required.")]
        [MaxLength(255)]
        public string DeliveryAddress { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
    }
}
