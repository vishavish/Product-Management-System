using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PMS.WebUI.Models.ProductInventoryViewModel
{
    public class ShipmentModel
    {
        public int Id { get; set; }
        public int Adjustment { get; set; }
        public IEnumerable<SelectListItem> ProductsListItem { get; set; } = new List<SelectListItem>();
    }
}
