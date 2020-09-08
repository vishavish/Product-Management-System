using PMS.Models.Domain;
using PMS.WebUI.Models.CustomerViewModel;

namespace PMS.WebUI.Mapper
{
    public static class CustomerMapper
    {
        public static CustomerModel SerializeCustomer(Customer customer)
        {
            return new CustomerModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PrimaryAddress = MapCustomerAddress(customer.PrimaryAddress),
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn
            };
        }

        public static Customer SerializeCustomer(CustomerModel customer)
        {
            return new Customer
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PrimaryAddress = MapCustomerAddress(customer.PrimaryAddress),
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn
            };
        }

        public static CustomerAddressModel MapCustomerAddress(CustomerAddress address)
        {
            return new CustomerAddressModel
            {
                Id = address.Id,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                Region = address.Region,
                PostalCode = address.PostalCode,
                Country = address.Country,
                CreatedOn = address.CreatedOn,
                UpdatedOn = address.UpdatedOn
            };
        }

        public static CustomerAddress MapCustomerAddress(CustomerAddressModel address)
        {
            return new CustomerAddress
            {
                Id = address.Id,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                Region = address.Region,
                PostalCode = address.PostalCode,
                Country = address.Country,
                CreatedOn = address.CreatedOn,
                UpdatedOn = address.UpdatedOn
            };
        }
    }
}
