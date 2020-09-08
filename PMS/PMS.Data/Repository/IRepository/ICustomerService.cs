using PMS.Models.Domain;
using System.Collections.Generic;

namespace PMS.Data.Repository.IRepository
{
    public interface ICustomerService
    {
        List<Customer> GetCustomers();
        Customer GetById(int id);
        bool CreateCustomer(Customer customer);
        bool DeleteCustomer(int id);
        bool UpdateCustomer(Customer customer);
        bool Save();
    }
}
