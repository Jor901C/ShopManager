using Infrastructure.Entities.Market;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopManager.DAL.Entities.User;
using ShopManager.DAL.Model;
using ShopManager.Services.Abstract;
using ShopManager.Web.Data;

namespace ShopManager.Services.Concrete
{
    //Implementation for Entity Framework
    public class EfManagerService : IManagerService
    {
        
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //DI
        public EfManagerService( ApplicationDbContext applicationDbContext,
            UserManager<UserModel> userManager , RoleManager<IdentityRole> roleManager)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        
        #region Managers functions
        //Add sales 
        public async Task<bool> AddSalesAsync(string managerId, int marketId, decimal revenue, DateOnly date)
        {
            using (var transaction = await _applicationDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if there are identical data in the database
                    bool existingSales = await _applicationDbContext.Sales.AnyAsync(m => m.MarketId == marketId && m.CreateDate == date);

                    // Ensure that the date is not later than today
                    var daysPassed = (DateTime.Now - new DateTime(date.Year, date.Month, date.Day)).TotalDays;

                    //Check if there are identical data in the database and ensure that the date is not later than today.
                    if (existingSales || daysPassed < 0)
                    {
                        return false;
                    }

                    var sales = new Sales()
                    {
                        Amount = revenue,
                        CreateDate = date,
                        MarketId = marketId,
                        UserId = managerId

                    };


                    // Add sales to the database
                    await _applicationDbContext.Sales.AddAsync(sales);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    // Handle exceptions here
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                    return false;
                }
            }
        }

        //To change sales for the last 30 days.
        public async Task<bool> ChangeSalesLastMounthAsync(int salesId, decimal newAmount, DateOnly newDate, int newMarketId)
        {
            // Begin a transaction to ensure atomicity of operations
            using (var transaction = await _applicationDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Find the sale by the given ID
                    var sale = await _applicationDbContext.Sales.FindAsync(salesId);

                    // Check if there are sales with the same date and market ID
                    bool testSales = await _applicationDbContext.Sales.AnyAsync(s => s.CreateDate == newDate && s.MarketId == newMarketId);

                    // If the sale exists and there are no other sales with the same date and market ID
                    if (sale != null && !testSales)
                    {
                        
                        sale.Amount = newAmount;
                        sale.CreateDate = newDate;
                        sale.MarketId = newMarketId;
                        sale.IsChanged++;
                        sale.UpdateDate = DateOnly.FromDateTime(DateTime.Now);
                       

                        // Check that the sale's creation date is not more than 30 days ago or the new date is not later than the current date
                        var daysChack1 = (DateTime.Now - new DateTime(sale.CreateDate.Year, sale.CreateDate.Month, sale.CreateDate.Day)).TotalDays;
                        var daysCheck2 = (DateTime.Now - new DateTime(newDate.Year, newDate.Month, newDate.Day)).TotalDays;

                        // If the date is not more than 30 days ago or the new date is not later than the current date, save the changes
                        if (daysChack1 <= 30 || daysCheck2 > 0)
                        {
                            await _applicationDbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return true;
                        }
                    }
                    throw new Exception();  
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        //Get Sales by Manager Id
        public async Task<ICollection<SalesModelView>> GetSalesByManagerAsync(string userId)
        {
            // Initialize a list to store sales views
            var salesViews = new List<SalesModelView>();

            try
            {
                // Retrieve sales data for the specified manager ID
                var salesManager = await _applicationDbContext.Sales
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.CreateDate) 
                    .ToListAsync();

                foreach (var sales in salesManager)
                {
                    // Retrieve market data for the sales record
                    var market = await _applicationDbContext.Market.FirstOrDefaultAsync(m => m.Id == sales.MarketId);

                    if (market != null)
                    {
                        // Create a new SalesModelView object
                        var salesView = new SalesModelView()
                        {
                            Id = sales.Id,
                            Amount = sales.Amount,
                            MarketName = market.Name,
                            Date = sales.CreateDate,
                            IsChanged = sales.IsChanged
                        };
                        if (sales.UpdateDate == DateOnly.MinValue)
                        {
                            salesView.ChangeDate = " ";
                        }
                        else
                        {
                            salesView.ChangeDate = sales.UpdateDate.ToString();
                        }

                        // Add the sales view to the list
                        salesViews.Add(salesView);
                    }
                }
                return salesViews;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return salesViews;
            }
        }

        #endregion

        #region Admins function
        // Markets functions

        //Add Market
        public async Task<bool> AddMarketAsync(string marketName, string marketAddress)
        {
            // Begin a new transaction for adding a market
            using (var transaction = await _applicationDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var market = new Market()
                    {
                        Name = marketName,
                        MarketAddress = new MarketAddress { Address = marketAddress }
                    };

                    // Add the market to the database and save changes
                    await _applicationDbContext.Market.AddAsync(market);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                    await transaction.RollbackAsync();
                    return false; 
                }
            }
        }

        //Show list  Market where isDalate false
        public async Task<ICollection<MarketModelView>> GetAllMarketAsync()
        {
            // Create a list to store market model views
            var viewMarketList = new List<MarketModelView>();
            try
            {
                var markets = await _applicationDbContext.Market
                    .Include(m => m.MarketAddress)
                    .Where(m => !m.IsDeleted) // Exclude deleted markets
                    .ToListAsync();

                foreach (var market in markets)
                {
                    // Add market model view to the list
                    viewMarketList.Add(new MarketModelView
                    {
                        Name = market.Name,
                        Address = market.MarketAddress.Address,
                        Id = market.Id
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving markets: {ex.Message}");
            }

            return viewMarketList;
        }

        //Deletion  from database 
        public async Task<bool> RemovedMarketFinallyAsync(int marketId)
        {
            try
            {
                // Find the market by its ID
                var market = await _applicationDbContext.Market.FindAsync(marketId);

                if (market != null)
                {
                    _applicationDbContext.Market.Remove(market);
                    await _applicationDbContext.SaveChangesAsync();
                    return true; 
                }

                // Market not found
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return false; 
            }
        }

        //Cencel chack the  IsDelete chackbox
        public async Task<bool> ReturnRemovedMarketAsync(int marketId)
        {
            try
            {
                // Find the market by its ID
                var market = await _applicationDbContext.Market.FindAsync(marketId);

                if (market != null)
                {
                    market.IsDeleted = false;
                    await _applicationDbContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return false;
            }
        }

        //Chack the box IsDelete
        public async Task<bool> RemoveMarketAsync(int marketId)
        {
            try
            {
                // Find the market to remove by its ID including market address
                var marketToRemove = await _applicationDbContext.Market
                    .Include(m => m.MarketAddress)
                    .FirstOrDefaultAsync(m => m.Id == marketId);

                if (marketToRemove != null)
                {
                    marketToRemove.IsDeleted = true;
                    marketToRemove.MarketAddress.IsDeleted = true;
                    await _applicationDbContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while removing market: {ex.Message}");
                return false;
            }
        }

        //Show list market where chackbox isDelete true
        public async Task<ICollection<MarketModelView>> GetRemovedMarketAsync()
        {
            // Create a list to store market model views
            var viewMarketList = new List<MarketModelView>();
            try
            {
                

                var markets = await _applicationDbContext.Market
                    .Include(m => m.MarketAddress)
                    .Where(m => m.IsDeleted) 
                    .ToListAsync();

                foreach (var market in markets)
                {
                    // Add market model view to the list
                    viewMarketList.Add(new MarketModelView
                    {
                        Name = market.Name,
                        Address = market.MarketAddress.Address,
                        Id = market.Id
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            return viewMarketList;
        }



        //Users fucntions

        //Assign the user the Manager role
        public async Task<bool> AddRollManagerAsync(string userId)
        { 
            try
            {
                string manager = "Manager";
                // Check if the role exists, if not, create it
                if (!await _roleManager.RoleExistsAsync(manager))
                {
                    await _roleManager.CreateAsync(new IdentityRole(manager));
                }

                // Find the user by ID
                var username = await _userManager.FindByIdAsync(userId);

                // If the user is found, assign the role and update IsManager flag
                if (username != null)
                {
                    await _userManager.AddToRoleAsync(username!, manager);
                    username.IsManager = true;
                    await _applicationDbContext.SaveChangesAsync();
                    
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return false;
            }
        }

        //Show list user where IsDelete false
        public async Task<ICollection<UserModelView>> GetAllUserAsync()
        {

            // Create a list to store user model views
            var viewModelList = new List<UserModelView>();
            try
            {

                // Retrieve all non-deleted users who are not managers from the database
                var allUser = await _applicationDbContext.ShopUser.Where(u => u.IsDeleted == false && u.IsManager == false).ToListAsync();

                foreach (var user in allUser)
                {
                    viewModelList.Add(new UserModelView
                    {
                        Name = user.Name,
                        MiddleName = user.MiddleName,
                        Email = user.Email!,
                        PhoneNumber = user.PhoneNumber!,
                        StringId = user.Id
                    });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            return viewModelList;

        }

        //Show list user where IsDelete false and IsManager True
        public async Task<ICollection<UserModelView>> GetManagersAsync()
        {
            // Create a list to store manager model views
            var viewModelList = new List<UserModelView>();
            try
            {
                

                // Retrieve all non-deleted managers from the database
                var allManagers = await _applicationDbContext.ShopUser.Where(u => u.IsDeleted == false && u.IsManager == true).ToListAsync();

                foreach (var manager in allManagers)
                {
                    viewModelList.Add(new UserModelView
                    {
                        Name = manager.Name,
                        MiddleName = manager.MiddleName,
                        Email = manager.Email!,
                        PhoneNumber = manager.PhoneNumber!,
                        StringId = manager.Id
                    });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            return viewModelList;

        }

        //Delete user from database
        public async Task<bool> RemoveUsersFinallyAsync(string userId)
        {
            try
            {
                var user = await _applicationDbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    _applicationDbContext.Users.Remove(user);
                    await _applicationDbContext.SaveChangesAsync();
                    return true; 
                }
                return false; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return false; 
            }
        }

        //Cencel chack the  IsDelete chackbox
        public async Task<bool> ReturnRemovedUsersAsync(string userId)
        {
            try
            {
                var user = await _applicationDbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    user.IsDeleted = false;
                    await _applicationDbContext.SaveChangesAsync();
                    return true; 
                }
                return false; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return false; 
            }
        }

        //Show users where IsDelete true
        public async Task<ICollection<UserModelView>> GetRemovedUsersAsync()
        {
            // Create a list to store user model views
            var viewModelList = new List<UserModelView>();
            try
            {
                // Retrieve all removed users from the database
                var allRemovedUser = await _applicationDbContext.ShopUser.Where(u => u.IsDeleted == true).ToListAsync();

                foreach (var user in allRemovedUser)
                {
                    viewModelList.Add(new UserModelView
                    {
                        Name = user.Name,
                        MiddleName = user.MiddleName,
                        Email = user.Email!,
                        PhoneNumber = user.PhoneNumber!,
                        StringId = user.Id
                    });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            return viewModelList;
        }

        //Chack the box Isdelete
        public async Task<bool> RemoveManagerAsync(string managerId)
        {
            try
            {
                // Find the manager by their ID
                var removeManager = await _applicationDbContext.ShopUser.FindAsync(managerId);

                if (removeManager != null)
                {
                    removeManager.IsDeleted = true;
                    await _applicationDbContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while removing manager: {ex.Message}");
                return false;
            }
        }



        //Sales functions

        //Show sales through filter
        public async Task<ICollection<SalesModelView>> GetRevenueFilterAsync(string?[] managerIds, int?[] marketIds, DateOnly fromDate, DateOnly toDate)
        {
            // Execute the query and retrieve sales data
            List<SalesModelView> saleses = new List<SalesModelView>();
            try
            {
                // Start with base query
                IQueryable<Sales> query = _applicationDbContext.Sales;

                // Filter by manager IDs if provided
                if (managerIds != null && managerIds.Length > 0)
                {
                    query = query.Where(s => managerIds.Contains(s.UserId));
                }

                // Filter by market IDs if provided
                if (marketIds != null && marketIds.Length > 0)
                {
                    query = query.Where(s => marketIds.Contains(s.MarketId));
                }

                // Filter by date range
                query = query.Where(s => s.CreateDate >= fromDate && s.CreateDate <= toDate);

                

                foreach (var que in await query.ToListAsync())
                {
                    var sales = new SalesModelView();
                    var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == que.UserId);
                    var market = await _applicationDbContext.Market.FirstOrDefaultAsync(m => m.Id == que.MarketId);
                    if (user != null)
                    {
                        sales.ManagerName = user.Name + " " + user.MiddleName;
                    }
                    if (market != null)
                    {
                        sales.MarketName = market.Name;
                    }
                    sales.Amount = que.Amount;
                    sales.Date = que.CreateDate;
                    saleses.Add(sales);
                }

                // Order the sales by date in descending order
                saleses = saleses.OrderByDescending(s => s.Date).ToList();

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            return saleses;
        }
        #endregion

































    }
}




