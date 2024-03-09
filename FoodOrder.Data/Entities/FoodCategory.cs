namespace FoodOrder.Data.Entities
{
    public class FoodCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Food> Foods { get; set; }
    }
}
