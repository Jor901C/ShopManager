using ShopManager.DAL.Model;

namespace ShopManager.Services.Abstract
{
    
    public interface IManagerService
    {
        //Adding, Updating, and Getting all sales of a particular manager
        #region For managers
        Task<bool> AddSalesAsync(string managerId, int marketId, decimal revenue, DateOnly date);
        Task<bool> ChangeSalesLastMounthAsync(int salesId, decimal newAmount, DateOnly newDate, int newMarketId);
        Task<ICollection<SalesModelView>> GetSalesByManagerAsync(string userId);
        #endregion

        //All Administrator Functions
        #region For admin
        //Actions with Markets
        Task<bool> AddMarketAsync(string marketName , string marketAddress);
        Task<bool> RemoveMarketAsync(int marketId);
        Task<ICollection<MarketModelView>> GetAllMarketAsync();
        Task<ICollection<MarketModelView>> GetRemovedMarketAsync();
        Task<bool> RemovedMarketFinallyAsync(int marketId);
        Task<bool> ReturnRemovedMarketAsync(int marketId);

        //Actions with Users
        Task<ICollection<UserModelView>> GetAllUserAsync();
        Task<bool> RemoveManagerAsync(string userId);
        Task<bool> AddRollManagerAsync(string userId);
        Task<ICollection<UserModelView>> GetManagersAsync();
        Task<ICollection<UserModelView>> GetRemovedUsersAsync();
        Task<bool> RemoveUsersFinallyAsync(string userId);
        Task<bool> ReturnRemovedUsersAsync(string userId);
        

        //Actions with Sales
        Task<ICollection<SalesModelView>> GetRevenueFilterAsync(string?[] managerIds, int?[] marketIds, DateOnly fromDate, DateOnly toDate);
       
       
      
        #endregion
    }
}
