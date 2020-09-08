using PMS.Models.Domain;
using System.Collections.Generic;

namespace PMS.Data.Repository.IRepository
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetProductByID(int id);
        bool CreateProduct(Product product);
        bool ArchiveProduct(int id);
        bool Save();
    }
}
