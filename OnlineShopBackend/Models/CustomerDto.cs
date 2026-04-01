using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.ComponentModel.DataAnnotations.Schema;


namespace OnlineShopBackend.Models
{
    [Table("CustomerDetails")]
    public class CustomerDto
    {
        #region Properties

        [Key]
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? RegistrationDate { get; set; }

        #endregion
    }
}
