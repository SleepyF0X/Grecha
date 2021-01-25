using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public sealed class Product
    {
        public int Id { get; set; }
        [Required] public string Shop { get; set; }
        public int IdFromStore { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string TradeMark { get; set; }
        public double Price { get; set; } //TODO switch to the fixed point
        [Required] public string Link { get; set; }
        public string Img { get; set; }
    }
}