namespace FoodOrder.Core.Models.Food
{
    public class FoodDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public FoodCategoryDto Category { get; set; }
    }

    public class FoodCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
