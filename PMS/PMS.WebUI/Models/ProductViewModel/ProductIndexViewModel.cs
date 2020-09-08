using PMS.WebUI.Models.ProductInventoryViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.WebUI.Models.ProductViewModel
{
    public class ProductIndexViewModel
    {
        public IEnumerable<ProductModel> ProductViewModels { get; set; }
        public ShipmentModel ShipmentModal { get; set; }
    }
}
