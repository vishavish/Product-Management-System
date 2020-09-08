using PMS.Data.Repository.IRepository;
using PMS.Models.Domain;
using PMS.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Data.Repository
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Product> GetAllProducts()
        {
            return _db.Products
                .Where(p => !p.IsArchived)
                .ToList();
        }

        public Product GetProductByID(int id)
        {
            return _db.Products.Find(id);
        }

        public bool ArchiveProduct(int id)
        {
            if (!ProductExists(id))
                throw new NotFoundException(nameof(Product), id);

            var product = _db.Products.Find(id);
            product.IsArchived = true;

            return Save();
        }

        public bool CreateProduct(Product product)
        {
            _db.Products.Add(product);

            var newInventory = new ProductInventory
            {
                Product = product,
                QuantityOnHand = 0,
                IdealQuantity = 10
            };

            _db.ProductInventories.Add(newInventory);

            return Save();
        }
        
        public bool Save()
        {
            return _db.SaveChanges() > 0 ? true: false;
        }

        private bool ProductExists(int id)
        {
            return _db.Products.Any(p => p.Id == id);
        }
    }
}
