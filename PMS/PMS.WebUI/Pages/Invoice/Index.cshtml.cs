using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PMS.Data.Repository.IRepository;

namespace PMS.WebUI.Pages.Invoice
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ICustomerService customerService, ILogger<IndexModel> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [BindProperty]
        public IndexViewModel IndexVM { get; set; }

        public void OnGet()
        {
            IndexVM = new IndexViewModel();
            IndexVM.CustomerList = _customerService.GetCustomers()
                .Select(customer => new SelectListItem
                {
                    Value = customer.Id.ToString(),
                    Text = customer.FirstName + " " + customer.LastName
                }).ToList();
        }

        public class IndexViewModel
        {
            public int CustomerId { get; set; }
            public List<SelectListItem> CustomerList { get; set; } = new List<SelectListItem>();
        }
    }
}
