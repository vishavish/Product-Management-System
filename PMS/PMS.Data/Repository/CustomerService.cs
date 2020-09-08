using Microsoft.EntityFrameworkCore;
using PMS.Data.Repository.IRepository;
using PMS.Models.Domain;
using PMS.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMS.Data.Repository
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _db;

        public CustomerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Customer GetById(int id)
        {
            return _db.Customers.Find(id);
        }

        public List<Customer> GetCustomers()
        {
            return _db.Customers
                .Include(c => c.PrimaryAddress)
                .OrderBy(c => c.LastName)
                .ToList();
        }

        public bool CreateCustomer(Customer customer)
        {
            _db.Customers.Add(customer);
            return Save();
        }
        
        public bool DeleteCustomer(int id)
        {
            var customer = _db.Customers.Find(id);

            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), id);
            }

            _db.Customers.Remove(customer);

            return Save();
        }
        
        public bool UpdateCustomer(Customer customer)
        {
            if(!CustomerExists(customer.Id))
            {
                throw new NotFoundException(nameof(Customer), customer.Id);
            }

            _db.Customers.Update(customer);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0 ? true : false;
        }

        private bool CustomerExists(int id)
        {
            return _db.Customers.Any(c => c.Id == id);
        }
    }
}
