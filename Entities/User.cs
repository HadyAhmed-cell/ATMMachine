using System.ComponentModel.DataAnnotations;

namespace ATMMachine.Entities
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(6, ErrorMessage = "Enter Valid PIN Number")]
        public int Pin { get; set; }

        [MaxLength(14, ErrorMessage = "Enter Valid Credit Card Number")]
        public long CreditCardNo { get; set; }

        public double AccountBalance { get; set; }
    }
}