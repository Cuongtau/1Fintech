using System.ComponentModel.DataAnnotations;

namespace Pay1Fintech.Model.Request
{
    public class Account
    {
        [Required]
        [MaxLength(100)]
        public string AccountName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
