using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderRelatedEntities
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string street, string city, string postalCode)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
        }

        public string Street { get; set; }
        public string City { get; set; }

        public string PostalCode { get; set; }

    }
}
