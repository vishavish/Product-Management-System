using PMS.WebUI.Models.CustomerViewModel;
using System;
using System.Collections.Generic;

namespace PMS.WebUI.Models.OrderViewModel
{
    public class OrderModel
    {
        public int Id { get; set; }
        public CustomerModel Customer { get; set; }
        public List<SalesOrderItemModel> SalesOrderItems { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
