using Infrastructure.Entities.Market;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopManager.DAL.Entities.User;

namespace ShopManager.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Adding all my data to this DbContext
        public DbSet<UserModel> ShopUser { get; set; }
        public DbSet<Market> Market { get; set; }
        public DbSet<MarketAddress> Address { get; set; }
        public DbSet<Sales> Sales { get; set; }
    }
}
