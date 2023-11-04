using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using testApi.Models;


namespace testApi.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
       
        private readonly CustomerDataAccessLayer objCustomer = new CustomerDataAccessLayer();

        [HttpGet (Name ="GetAllCustomers")]
        // Get all Customers in the Database
        public IActionResult GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            customers = objCustomer.GetAllCustomers().ToList();
            return new JsonResult(customers);
        }

        [HttpPost]
        // create a new Customer
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {  
            objCustomer.AddCustomer(customer);
            return Ok(new { Message = "Customer Record Added" });

        }

        // Edit an existing customer
        [Route("Edit")]
        [HttpPost]
        public IActionResult EditCustomer([FromBody] Customer customer)
        {
            objCustomer.UpdateCustomer(customer);
            return Ok(new { Message = "Customer Record Edited" });
        }

        // Delete an existing Customer
        [Route("Delete")]
        [HttpDelete]
        public IActionResult DeleteCustomer([FromBody] Guid id)
        {
            objCustomer.DeleteCustomer(id);
            return Ok(new { Message = "Customer Record Deleted" });
        }

        // Get all active orders by a Customer
        [HttpGet("{customerId}/active-orders")]
        public IActionResult GetActiveOrdersByCustomer(Guid customerId)
        {
            List<OrderWithDetails> activeOrders = objCustomer.GetActiveOrdersByCustomer(customerId);
            return new JsonResult(activeOrders);
        }

    }
}
