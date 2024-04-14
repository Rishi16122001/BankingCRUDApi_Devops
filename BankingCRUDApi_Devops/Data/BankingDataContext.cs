using BankingCRUDApi_Devops.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingCRUDApi_Devops.Data
{
    public class BankingDataContext:DbContext
    {
        public BankingDataContext(DbContextOptions<BankingDataContext> options): base(options) 
        {
                
        }

        public DbSet<CustomerModel> CustomerDetails { get; set; }

    }
}
