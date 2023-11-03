namespace testApi.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int OrderStatus { get; set; }
        public int OrderType { get; set; }
        public DateTime OrderedOn { get; set; }
        public DateTime? ShippedOn { get; set; }    // shipped on can be null since the order may be waiting to get shipped
        public bool IsActive { get; set; }
    }
}
