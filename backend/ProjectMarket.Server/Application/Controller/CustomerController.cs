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
        try
        {
            Customer customer = repository.GetByCustomerId(id);
            return Ok(customer);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        catch(Exception)
        {
            return BadRequest("Customer couldn't be retrieved.");
        }
    }


    // TODO: Must refactor repository to return Id.
    [HttpPost]
    public ActionResult<Customer> PostCustomer([FromBody] CustomerDto dto)
    {
        Customer inserted;
        using(_customerRepository.uow) 
        {
            try
            {
                dto.Validate();
                inserted = _customerRepository.Insert(dto);
                _customerRepository.uow.Commit();
            }
            catch (ValidationException)
            {
                return BadRequest("Body is on a invalid state.");
            }
            catch(SqlException)
            {
                _customerRepository.uow.Rollback();
                return BadRequest("Error saving customer.");
            }
        }
        return CreatedAtAction(nameof(GetCustomerById), new { id = inserted.CustomerId }, inserted);
    }

    // TODO: Must refactor repository to return Id.
    [HttpPut("{id:int}")]
    public ActionResult<Customer> UpdateCustomer([FromRoute] int id, [FromBody] CustomerDto dto)
    {
        Customer updated;
        using(_customerRepository.uow) 
        {
            try 
            {
                _customerRepository.GetByCustomerId(id);
                Customer customer = Customer.CreateCustomer(dto);
                updated = _customerRepository.Update(customer);
                _customerRepository.uow.Commit();
            }
            catch(ArgumentException)
            {
                return NotFound();
            }
            catch (ValidationException)
            {
                return BadRequest("Body is on a invalid state.");
            }
            catch(SqlException)
            {
                _customerRepository.uow.Rollback();
                return BadRequest("Error updating customer.");
            }
        }
        return CreatedAtAction(nameof(GetCustomerById), new { id = updated.CustomerId }, updated);
    }
}
