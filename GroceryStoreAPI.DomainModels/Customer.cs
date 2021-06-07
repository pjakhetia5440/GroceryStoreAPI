using System;
using System.Net;

namespace GroceryStoreAPI.DomainModels
{
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class GeneralInput
    {
        public string Name { get; set; }
        //Can add more properties in future.
    }
    public class GeneralOutput
    {
        public int Id { get; set; }
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
