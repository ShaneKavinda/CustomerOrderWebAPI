using System;

namespace testApi.Models
{
    public class OrderWithDetails
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int OrderStatus { get; set; }
        public int OrderType { get; set; }
        public DateTime OrderedOn { get; set; }
        public DateTime? ShippedOn { get; set; } // Nullable DateTime
        public bool IsActive { get; set; }

        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
