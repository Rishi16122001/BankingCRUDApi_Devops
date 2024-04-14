using BankingCRUDApi_Devops.Data;
using BankingCRUDApi_Devops.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingCRUDApi_Devops.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingController : ControllerBase
    {
        private readonly BankingDataContext _context;

        public BankingController(BankingDataContext context)
        {
            _context = context;
        }


        [HttpGet("all-customers")]
        public async Task<ActionResult<List<CustomerModel>>> GetAllCustomerDetails()
        {
            return await _context.CustomerDetails.ToListAsync();
        }


        [HttpGet("details/{id}")]
        public async Task<ActionResult<CustomerModel>> GetCustomerDetailsWithId(int id)
        {
            var specificDetails = await _context.CustomerDetails.FindAsync(id);
            if (specificDetails == null)
            {
                return NotFound("Book not found!");
            }
            return Ok(specificDetails);
        }

        [HttpPost("add-customer")]
        public async Task<ActionResult<List<CustomerModel>>> AddCustomer(CustomerModel customer)
        {
            _context.CustomerDetails.Add(customer);
            await _context.SaveChangesAsync();
            var updatedCustomerList = await _context.CustomerDetails.ToListAsync();
            return Ok(updatedCustomerList);
        }

        [HttpDelete("delete-customer")]
        public async Task<ActionResult<List<CustomerModel>>> DeleteCustomer(int customerId)
        {
            var customer = await _context.CustomerDetails.FindAsync(customerId);
            if (customer == null)
            {
                return NotFound("Customer with this Id not found!");
            }
            _context.CustomerDetails.Remove(customer);
            await _context.SaveChangesAsync();
            return Ok(customer);

        }


        [HttpPut("update-customer")]
        public async Task<ActionResult<CustomerModel>> UpdateCustomer(CustomerModel customer)
        {

            var existingCustomer = await _context.CustomerDetails.FindAsync(customer.Id);
            if (existingCustomer == null)
            {
                return NotFound("Customer not found");
            }

            existingCustomer.CustomerName = customer.CustomerName;
            existingCustomer.AccountNumber = customer.AccountNumber;
            existingCustomer.CustomerEmail = customer.CustomerEmail;
            existingCustomer.CustomerCity = customer.CustomerCity;

            await _context.SaveChangesAsync();

            return Ok(await _context.CustomerDetails.ToListAsync());
        }




    }
}
