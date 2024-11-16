using System.ComponentModel.DataAnnotations;

namespace ChoicesExtrasManagement.Models
{
    public class PaymentTransaction
    {
        [Key]
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }

        public decimal Amount { get; set; }

    }
}
