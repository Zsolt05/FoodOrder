namespace FoodOrder.Core.Models.Food
{
    public class FoodDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public FoodCategoryDto Category { get; set; }
    }

    public class CreateFoodDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }

    public class FoodCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FoodOrderDto
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
