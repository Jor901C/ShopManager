using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Market
{
    public class Market
    {
        [Key]
        public int Id { get; set; }
        [Required]
        
        public string Name { get; set; } = string.Empty;
        [ForeignKey("MarketAddress")]
        public int AddressId { get; set; }
        public virtual MarketAddress MarketAddress { get; set; } = null!; 
        public ICollection<Sales>? Saleses {  get; set; } 
        public bool IsDeleted { get; set; }
    }
}
