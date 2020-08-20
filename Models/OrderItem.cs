namespace FeedMe.Models
{
    public class OrderItem
    {
        public int OrderFK { get; set; }
        public int ItemFK { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
    }
}