using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManager.DAL.Model
{
    public class MarketModelView
    {
        public int Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address {  get; set; } = string.Empty ;
    }
}
