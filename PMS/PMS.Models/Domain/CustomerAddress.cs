using System;
using System.ComponentModel.DataAnnotations;

namespace PMS.Models.Domain
{
    public class CustomerAddress
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string AddressLine1 { get; set; }

        [MaxLength(100)]
        public string AddressLine2 { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Region { get; set; }

        [MaxLength(10)]
        public string PostalCode { get; set; }

        [MaxLength(32)]
        public string Country { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
