namespace DAL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Shop { get; set; }
        public int IdFromShop { get; set; }
        public string Name { get; set; }
        public string TradeMark { get; set; }
        public double Price { get; set; } //TODO switch to the fixed point
    }
}