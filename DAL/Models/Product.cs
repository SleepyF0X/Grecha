using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public sealed class Product
    {
        public int Id { get; set; }
        [Required] public string StoreName { get; set; }
        public int IdFromStore { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string TradeMark { get; set; }
        public double Price { get; set; } //TODO switch to the fixed point
        public string PhotoPath { get; set; }
        [Required] public string Link { get; set; }
    }
}