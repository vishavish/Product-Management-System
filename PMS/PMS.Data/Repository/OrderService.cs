using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMS.Data.Repository.IRepository;
using PMS.Models.Domain;
using PMS.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Data.Repository
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _db;
        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            ApplicationDbContext db, 
            ILogger<OrderService> logger, 
            IInventoryService inventoryService, 
            IProductService productService)
        {
            _db = db;
            _inventoryService = inventoryService;
            _productService = productService;
            _logger = logger;
        }
        
        public List<SalesOrder> GetSalesOrders()
        {
            return _db.SalesOrders
                .Include(so => so.Customer)
                    .ThenInclude(c => c.PrimaryAddress)
                .Include(so => so.SalesOrderItems)
                    .ThenInclude(item => item.Product)
                .ToList();
        }

        public bool GenerateOpenOrder(SalesOrder order)
        {
            var now = DateTime.UtcNow;

            foreach (var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProductByID(item.Product.Id);

                var inventoryId = _inventoryService.GetById(item.Product.Id).Id;

                _inventoryService.UpdateUnitsAvailable(inventoryId, -item.Quantity);
            }

            _db.SalesOrders.Add(order);
            return Save();
        }

        public bool MarkFulfilled(int id)
        {
            if (!OrderExists(id)) throw new NotFoundException(nameof(SalesOrder), id);

            var now = DateTime.UtcNow;
            var salesOrder = _db.SalesOrders.Find(id);

            salesOrder.IsPaid = true;
            salesOrder.UpdatedOn = now;

            _db.SalesOrders.Update(salesOrder);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 1 ? true : false;
        }

        private bool OrderExists(int id)
        {
            return _db.SalesOrders.Any(o => o.Id == id);
        }
    }
}
