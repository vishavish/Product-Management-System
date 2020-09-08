using PMS.Models.Domain;
using System.Collections.Generic;

namespace PMS.Data.Repository.IRepository
{
    public interface IInventoryService
    {
        List<ProductInventory> GetProductInventories();
        ProductInventory GetById(int productId);
        List<ProductInventorySnapshot> GetSnapshotHistory();
        bool UpdateUnitsAvailable(int id, int adjustment);
        bool Save();
    }
}
