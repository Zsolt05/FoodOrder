namespace FoodOrder.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual List<CartFood> CartFood { get; set; }
        public CartStatus Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }

    public enum CartStatus
    {
        Open,
        Finished
    }
}