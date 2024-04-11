using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities.Market
{
    public class MarketAddress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        //Mark for deletion.
        public bool IsDeleted { get; set; }

    }
}
