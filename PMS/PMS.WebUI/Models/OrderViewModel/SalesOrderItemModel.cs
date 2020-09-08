using PMS.WebUI.Models.ProductViewModel;

namespace PMS.WebUI.Models.OrderViewModel
{
    public class SalesOrderItemModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ProductModel Product { get; set; }
    }
}
