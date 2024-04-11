namespace ShopManager.DAL.Model
{
    public class SalesModelView
    {
        public int Id { get; set; } 
        public string? ManagerName { get; set; } 
        public string? MarketName { get; set;}
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string? ChangeDate {  get; set; } 
        public int IsChanged { get; set; }

    }
}
