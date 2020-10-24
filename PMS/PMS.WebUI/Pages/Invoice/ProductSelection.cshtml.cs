using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Data.Repository.IRepository;
using PMS.WebUI.Extensions;
using PMS.WebUI.Mapper;
using PMS.WebUI.Models.OrderViewModel;

namespace PMS.WebUI.Pages.Invoice
{
    public class ProductSelectionModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductSelectionModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductSelectionViewModel ProductVM { get; set; } 

        public void OnGet()
        {
            ProductVM = new ProductSelectionViewModel();
            ProductVM.ProductList = _productService.GetAllProducts()
                .Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }).ToList();
        }

        public IActionResult OnPostAddLineItem()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("ProductSelection");
            }

            var product = _productService.GetProductByID(ProductVM.Id);
            var prodModel = ProductMapper.SerializeProductModel(product);
            SalesOrderItemModel soModel = new SalesOrderItemModel
            {
                Quantity = ProductVM.Quantity,
                Product = prodModel
            };

            if (!IsExisting(product.Id))
            {
                AddItemToList(soModel);
            }
            else
            {
                UpdateQuantity(product.Id, soModel.Quantity);
            }

            return RedirectToPage("ProductSelection");
        }

        public IActionResult OnPostFinalizeOrder()
        {
            return Page();
        }

        private void AddItemToList(SalesOrderItemModel soModel)
        {
            List<SalesOrderItemModel> salesOrderItems = GetSalesOrderItemList();

            salesOrderItems.Add(soModel);
            HttpContext.Session.Set("SalesOrderItems", salesOrderItems);
        }

        private List<SalesOrderItemModel> GetSalesOrderItemList()
        {
            var salesOrderItemList = HttpContext.Session.Get<List<SalesOrderItemModel>>("SalesOrderItems");
            return salesOrderItemList == null ? new List<SalesOrderItemModel>() : salesOrderItemList;
        }

        private void UpdateQuantity(int productId, int quantity)
        {
            var salesOrderItemList = GetSalesOrderItemList();
            var salesOrdertItemModel = salesOrderItemList.FirstOrDefault(i => i.Product.Id == productId);

            salesOrdertItemModel.Quantity += quantity;

            HttpContext.Session.Set("SalesOrderItems", salesOrderItemList);
        }

        private bool IsExisting(int id)
        {
            return GetSalesOrderItemList() == null ? false : GetSalesOrderItemList().Any(i => i.Product.Id == id);
        }
    }

    public class ProductSelectionViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public List<SelectListItem> ProductList { get; set; } = new List<SelectListItem>();
        public List<SalesOrderItemModel> SalesOrderItems { get; set; } = new List<SalesOrderItemModel>();
    }
}
