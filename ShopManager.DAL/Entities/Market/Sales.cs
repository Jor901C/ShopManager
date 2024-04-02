using ShopManager.DAL.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Market
{
    public class Sales
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a non-negative number")]
        public decimal Amount { get; set; }
        [Required]
        public DateOnly CreateDate {  get; set; } 
        [Required]
        public DateOnly UpdateDate { get; set; }
        [Required]
        public int IsChanged { get; set; }
        [ForeignKey("UserModel")]
        public string UserId { get; set; } = null!;
        public virtual UserModel Users { get; set; } = null!;
        [ForeignKey("Market")]
        public int MarketId { get; set; }
        public virtual Market Market { get; set; } = null!;
       


    }
}
