using Infrastructure.Entities.Market;
using ShopManager.DAL.Entities.User;
using ShopManager.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager.Services.Abstract
{
    public interface IManagerService
    {
        Task AddSalesAsync(Sales sales);
        Task AddMarketAsync(string marketName , string marketAddress);
        Task RemoveMarketAsync(int marketId);
        Task<ICollection<MarketModelView>> GetAllMarketAsync();
        Task<ICollection<Sales>> GetAllSalesAsync();
        Task<ICollection<Sales>> GetSalesByManagerAsync(string userId);
        Task <ICollection<Sales>> GetSalesByMarketAsync(int marketId);
        Task <ICollection <Sales>> GetSalesByDayAsync(DateOnly dateOnly);
        Task<ICollection<UserModelView>> GetAllUser();
        Task ChangeSalesLastMounthAsync(int salesId , decimal newaAmount);
        Task RedactorMarketDataAsync(int market , string newName);
        Task RemoveManagerAsync(string userId);
        Task <ICollection<Market>> GetRemovedMarket();
        Task AddRollAsync(string userId , string manager);
        Task <ICollection<UserModelView>> GetOnlyManager();

    }
}
