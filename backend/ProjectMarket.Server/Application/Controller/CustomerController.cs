using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProjectMarket.Server.Data.Model.DTO;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Application.Controller;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly CustomerRepository _customerRepository;

    public CustomersController(IUnitOfWork uow) {
        _uow = uow;
        _customerRepository = new CustomerRepository(_uow);
    }

    [HttpGet("{id}")]
    public ActionResult<Customer> GetCustomerById(int id, [FromServices] CustomerRepository repository)
    {
        Customer? customer = repository.GetByCustomerId(id);

        if (customer == null)
            return NotFound();
        else
            return Ok(customer);
    }


    // TODO: Must refactor repository to return Id.
    [HttpPost]
    public ActionResult<Customer> PostCustomer([FromBody] CustomerDTO customerDTO)
    {
        Customer customer; 
        try 
        {
            customer = Customer.CreateCustomer(customerDTO);
        }
        catch(ValidationException e)
        {
            return BadRequest("Body is on a invalid state.");
        }

        Customer createdCustomer;
        using(_customerRepository.uow) 
        {
            try 
            {
                _customerRepository.Insert(customer);
                _customerRepository.uow.Commit();
            }
            catch(Exception e)
            {
                _customerRepository.uow.Rollback();
                return BadRequest("Error saving customer.");
            }
        }

        // Return the created customer with a 201 status code
        // return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
    }

}
