using System;
using System.Collections.Generic;

namespace PMS.WebUI.Models.OrderViewModel
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<SalesOrderItemModel> LineItems { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public InvoiceModel()
        {
            LineItems = new List<SalesOrderItemModel>();
        }
    }
}
