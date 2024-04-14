using System.ComponentModel.DataAnnotations;

namespace BankingCRUDApi_Devops.Models
{
    public class CustomerModel
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public long AccountNumber { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerCity { get; set; } = string.Empty;
    }
}
