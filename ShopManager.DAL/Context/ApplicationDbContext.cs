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
        public DbSet<UserModel> ShopUsers { get; set; }
        public DbSet<Market> Market { get; set; }
        public DbSet<MarketAddress> Addres { get; set; }
        public DbSet<Sales> Sales { get; set; }
    }
}
