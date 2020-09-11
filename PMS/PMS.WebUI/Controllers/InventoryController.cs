using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMS.Data.Repository.IRepository;
using PMS.WebUI.Mapper;
using PMS.WebUI.Models.ProductInventoryViewModel;
using System.Linq;

namespace PMS.WebUI.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateUnitsAvailable(ShipmentModel shipmentModel)
        {
            var res = _inventoryService.UpdateUnitsAvailable(shipmentModel.Id, shipmentModel.Adjustment);

            return RedirectToAction(nameof(Index), "Product");
        }

        private void GetCurrentInventory()
        {
            _logger.LogInformation("Getting current inventory");
            var inventory = _inventoryService.GetProductInventories()
                .Select(pi => new ProductInventoryModel
                {
                    Id = pi.Id,
                    IdealQuantity = pi.IdealQuantity,
                    QuantityOnHand = pi.QuantityOnHand,
                    Product = ProductMapper.SerializeProductModel(pi.Product)
                })
                .OrderBy(pi => pi.Product.Name)
                .ToList();
        }
    }
}
