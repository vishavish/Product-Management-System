using System;

namespace PMS.Models.Domain
{
    public class ProductInventory
    {
        public int Id { get; set; }
        public int QuantityOnHand { get; set; }
        public int IdealQuantity { get; set; }
        public Product Product { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
