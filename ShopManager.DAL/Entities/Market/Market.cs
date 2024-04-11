using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities.Market
{
    //The store data and foreign keys, the store address in a separate class.
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

        //Mark for deletion.
        public bool IsDeleted { get; set; }
    }
}
