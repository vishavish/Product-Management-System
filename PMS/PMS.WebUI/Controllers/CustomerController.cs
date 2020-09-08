using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMS.Data.Repository.IRepository;
using PMS.WebUI.Mapper;
using PMS.WebUI.Models.CustomerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.WebUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var customerModels = GetAllCustomers();

            return View(customerModels);
        }

        [HttpPost]
        public IActionResult CreateCustomer(CustomerModel customer)
        {
            _logger.LogInformation("Creating new customer...");
            var now = DateTime.UtcNow;

            customer.CreatedOn = now;
            customer.UpdatedOn = now;

            var customerData = CustomerMapper.SerializeCustomer(customer);
            if (!_customerService.CreateCustomer(customerData))
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteCustomer(int id)
        {
            _logger.LogInformation("Deleting a customer...");

            if (!_customerService.DeleteCustomer(id))
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCustomer()
        {
            return View();
        }

        private List<CustomerModel> GetAllCustomers()
        {
            return _customerService.GetCustomers()
                .Select(customer => new CustomerModel
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PrimaryAddress = CustomerMapper.MapCustomerAddress(customer.PrimaryAddress),
                    CreatedOn = customer.CreatedOn,
                    UpdatedOn = customer.UpdatedOn
                })
                .ToList();
        }
    }
}
