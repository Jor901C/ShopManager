using ShopManager.DAL.Entities.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        //Date of apologies
        public DateOnly UpdateDate { get; set; }

        //Number of apologies
        [Required]
        public int IsChanged { get; set; } 

        [ForeignKey("UserModel")]
        public string UserId { get; set; } = null!;

        public virtual UserModel User { get; set; } = null!;

        [ForeignKey("Market")]
        public int MarketId { get; set; }

        public virtual Market Market { get; set; } = null!;
       


    }
}
