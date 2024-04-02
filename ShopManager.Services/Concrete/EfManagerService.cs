using Infrastructure.Entities.Market;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopManager.DAL.Entities.User;
using ShopManager.DAL.Model;
using ShopManager.Services.Abstract;
using ShopManager.Web.Data;

namespace ShopManager.Services.Concrete
{
    public class EfManagerService : IManagerService
    {
        
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public EfManagerService( ApplicationDbContext applicationDbContext,
            UserManager<UserModel> userManager , RoleManager<IdentityRole> roleManager)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        


        public async  Task AddMarketAsync(string marketName , string marketAddress)
        {
            var market = new Market()
            {
                Name = marketName,
                MarketAddress = new MarketAddress { Address = marketAddress}
            };

            await _applicationDbContext.Market.AddAsync(market);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task AddRollAsync(string userId , string manager)
        {
            if (!await _roleManager.RoleExistsAsync(manager))
            {
                await _roleManager.CreateAsync(new IdentityRole(manager));
            }

            var username = await _userManager.FindByIdAsync(userId);
            if(username!=null)
            {
                await _userManager.AddToRoleAsync(username!, manager);
                username.IsManager = true;
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task AddSalesAsync(Sales sales)
        {

            await _applicationDbContext.Sales.AddAsync(sales);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task ChangeSalesLastMounthAsync(int salesId , decimal newAmount)
        {
            var sale = await _applicationDbContext.Sales.FindAsync(salesId);
            if(sale!=null)
            {
                sale.Amount = newAmount;
                sale.UpdateDate = DateOnly.FromDateTime(DateTime.Now);
                _applicationDbContext.SaveChanges();
            }
            
        }

        public async Task<ICollection<MarketModelView>> GetAllMarketAsync()
        {
            var viewMarketList = new List<MarketModelView>();
            var markets = await _applicationDbContext.Market
             .Include(m => m.MarketAddress) 
             .Where(m => !m.IsDeleted)
             .ToListAsync();


            foreach (var market in markets)
            {

                viewMarketList.Add(new MarketModelView
                {
                    Name = market.Name,
                    Address = market.MarketAddress.Address,
                    Id = market.Id
                });
            }
            return viewMarketList;
        }

        public async Task<ICollection<Sales>> GetAllSalesAsync()
        {
            var saleses = await _applicationDbContext.Sales.ToListAsync();
            return saleses;
        }

        public async Task<ICollection<UserModelView>> GetAllUser()
        {
            var viewModelList = new List<UserModelView>();
            var allUser = await _applicationDbContext.ShopUsers.Where(u=>u.IsDeleted==false && u.IsManager == false).ToListAsync();
            foreach (var user in allUser)
            {
                viewModelList.Add(new UserModelView
                {
                    Name = user.Name,
                    MiddleName = user.MiddleName,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber!,
                    stringId   = user.Id
                    
                });
                
            }

            return viewModelList;
        }

        public async Task<ICollection<UserModelView>> GetOnlyManager()
        {
            var viewModelList = new List<UserModelView>();
            var allUser = await _applicationDbContext.ShopUsers.Where(u => u.IsDeleted == false && u.IsManager == true).ToListAsync();
            foreach (var user in allUser)
            {
                viewModelList.Add(new UserModelView
                {
                    Name = user.Name,
                    MiddleName = user.MiddleName,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber!,
                    stringId = user.Id

                });
            }

            return viewModelList;
        }

        public async Task<ICollection<Market>> GetRemovedMarket()
        {
            var removedMarket = await _applicationDbContext.Market.Where(m => m.IsDeleted == true).ToListAsync();
            return removedMarket;
        }

        public async  Task<ICollection<Sales>> GetSalesByDayAsync(DateOnly dateOnly)
        {
            var salesByDay = await _applicationDbContext.Sales.Where(s => s.CreateDate == dateOnly).ToListAsync();
            return salesByDay;
        }

        public async Task<ICollection<Sales>> GetSalesByManagerAsync(string userId)
        {
            var salesManager = await _applicationDbContext.Sales.Where(s=>s.UserId == userId).ToListAsync();
            return salesManager;
        }

        public async Task<ICollection<Sales>> GetSalesByMarketAsync(int marketId)
        {
            var salesByMarket = await _applicationDbContext.Sales.Where(s => s.MarketId == marketId).ToListAsync();
            return salesByMarket;
        }

        public async Task RedactorMarketDataAsync(int marketId , string newName)
        {
            var marketRedactor = await _applicationDbContext.Market.FindAsync(marketId);
            if (marketRedactor != null)
            {
                marketRedactor.Name = newName;
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveManagerAsync(string managerId)
        {
            var removeManager = await _applicationDbContext.ShopUsers.FindAsync(managerId);
            if(removeManager!=null)
            {
                removeManager.IsDeleted = true;
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveMarketAsync(int marketId)
        {
            var marketToRemove = await _applicationDbContext.Market
                  .Include(m => m.MarketAddress) 
                  .FirstOrDefaultAsync(m => m.Id == marketId);
            if (marketToRemove != null)
            {
                marketToRemove.IsDeleted = true;
                marketToRemove.MarketAddress.IsDeleted = true;
                await _applicationDbContext.SaveChangesAsync();
            }
            
        }
       

    }
}
