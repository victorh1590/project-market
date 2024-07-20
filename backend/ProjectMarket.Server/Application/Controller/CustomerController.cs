using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Application.Controller;

[ApiController]
[Route("[controller]")]
public class CustomersController(IUnitOfWork uow) : ControllerBase
{
    private readonly CustomerRepository _customerRepository = new(uow);

    [HttpGet("{id:int}")]
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
    public ActionResult<Customer> PostCustomer([FromBody] CustomerDto dto)
    {
        try 
        {
            dto.Validate();
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
                _customerRepository.Insert(dto);
                _customerRepository.uow.Commit();
            }
            catch(Exception)
            {
                _customerRepository.uow.Rollback();
                return BadRequest("Error saving customer.");
            }
        }

        // Return the created customer with a 201 status code
        // return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
    }

    // TODO: Must refactor repository to return Id.
    [HttpPut("{id:int}")]
    public ActionResult<Customer> UpdateCustomer(int id, [FromBody] CustomerDto dto)
    {
        try
        {
            _customerRepository.GetByCustomerId(id);
        }
        catch(SqlException)
        {
            return NotFound();
        }

        Customer customer;
        try 
        {
            customer = Customer.CreateCustomer(dto);
        }
        catch (ValidationException)
        {
            return BadRequest("Body is on a invalid state.");
        }

        Customer createdCustomer;
        using(_customerRepository.uow) 
        {
            try 
            {
                _customerRepository.Update(customer);
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
