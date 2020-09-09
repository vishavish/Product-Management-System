using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMS.Data.Repository.IRepository;
using PMS.WebUI.Mapper;
using PMS.WebUI.Models.OrderViewModel;
using System.Collections.Generic;

namespace PMS.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ICustomerService customerService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _customerService = customerService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var orderModels = GetOrderModels();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateNewOrder(InvoiceModel invoiceModel)
        {
            _logger.LogInformation("Generating invoice...");

            var order = OrderMapper.SerializeInvoiceToOrder(invoiceModel);
            order.Customer = _customerService.GetById(invoiceModel.CustomerId);
            _orderService.GenerateOpenOrder(order);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkOrderComplete(int id)
        {
            _logger.LogInformation($"Marking order {id} as fulfilled.");
            if (!_orderService.MarkFulfilled(id))
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }
        
        private List<OrderModel> GetOrderModels()
        {
            var orders = _orderService.GetSalesOrders();
            return OrderMapper.SerializeOrdersToViewModel(orders);
        }
    }
}
