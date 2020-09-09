using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMS.Data.Repository.IRepository;
using PMS.Models.Domain;
using PMS.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Data.Repository
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _db;
        private ILogger<InventoryService> _logger;

        public InventoryService(ApplicationDbContext db, ILogger<InventoryService> logger)
        {
            _db = db;
            _logger = logger;
        }
        
        public List<ProductInventory> GetProductInventories()
        {
            return _db.ProductInventories
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product.IsArchived)
                .ToList();
        }

        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);

            return _db.ProductInventorySnapshots
                .Include(p => p.Product)
                .Where(p => p.SnapshotTime > earliest
                          && !p.Product.IsArchived)
                .ToList();
        }

        public ProductInventory GetById(int productId)
        {
            return _db.ProductInventories
                .Include(pi => pi.Product)
                .FirstOrDefault(pi => pi.Product.Id == productId);
        }
        
        public bool UpdateUnitsAvailable(int id, int adjustment)
        {
            if (!ProductInventoryExists(id)) throw new NotFoundException(nameof(ProductInventory), id);

            var productInventory = GetById(id);
            
            productInventory.QuantityOnHand += adjustment;
            CreateSnapshot(productInventory);

            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 1 ? true : false;
        }

        private void CreateSnapshot(ProductInventory inventory)
        {
            var now = DateTime.UtcNow;

            var snapshot = new ProductInventorySnapshot
            {
                SnapshotTime = now,
                Product = inventory.Product,
                QuantityOnHand = inventory.QuantityOnHand
            };

            _db.ProductInventorySnapshots.Add(snapshot);
        }

        private bool ProductInventoryExists(int id)
        {
            return _db.ProductInventories
                .Include(pi => pi.Product)
                .Any(pi => pi.Product.Id == id);
        }
    }
}
