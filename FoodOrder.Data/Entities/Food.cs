using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrder.Data.Entities
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public FoodCategory Category { get; set; }
    }
}
