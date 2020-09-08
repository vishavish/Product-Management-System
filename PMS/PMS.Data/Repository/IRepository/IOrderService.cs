using PMS.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMS.Data.Repository.IRepository
{
    public interface IOrderService
    {
        List<SalesOrder> GetSalesOrders();
        bool GenerateOpenOrder(SalesOrder order);
        bool MarkFulfilled(int id);
        bool Save();
    }
}
