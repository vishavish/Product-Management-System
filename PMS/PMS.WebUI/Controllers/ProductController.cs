using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PMS.Data.Repository.IRepository;
using PMS.WebUI.Mapper;
using PMS.WebUI.Models.ProductInventoryViewModel;
using PMS.WebUI.Models.ProductViewModel;
using System.Linq;

namespace PMS.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<IProductService> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<IProductService> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Getting all products...");
            var products = _productService.GetAllProducts()
                .Select(ProductMapper.SerializeProductModel);

            var selectListItem = _productService.GetAllProducts()
                .Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });

            var indexViewModels = new ProductIndexViewModel
            {
                ProductViewModels = products,
                ShipmentModal = new ShipmentModel
                {
                    ProductsListItem = products
                        .Select(item => new SelectListItem
                        {
                            Text = item.Name,
                            Value = item.Id.ToString()
                        })
                }
            };

            return View(indexViewModels);
        }

        public IActionResult Detail(int id)
        {
            var product = _productService.GetProductByID(id);

            if (product == null)
                return NotFound();
            
            var productModel = ProductMapper.SerializeProductModel(product);
            
            return View(productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductModel product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var prodModel = ProductMapper.SerializeProductModel(product);

            if(!_productService.CreateProduct(prodModel))
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation($"Archiving product with id:{id}.");

            if (!_productService.ArchiveProduct(id))
                return BadRequest();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
