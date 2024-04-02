using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Market
{
    public class MarketAddress
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Address { get; set; } = string.Empty;
        
        public bool IsDeleted { get; set; }

    }
}
