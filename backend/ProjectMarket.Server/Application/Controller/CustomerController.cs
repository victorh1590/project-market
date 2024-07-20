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
    public ActionResult<Customer> GetCustomerById(int id)
    {
        try
        {
            return Ok(_customerRepository.GetByCustomerId(id));
        }
        catch (ArgumentException)
        {
            return NotFound($"{nameof(Customer)} with {nameof(id)} {id} not found.");
        }
        catch(Exception)
        {
            return BadRequest($"{nameof(Customer)} couldn't be retrieved.");
        }
    }

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
            catch(Exception)
            {
                _customerRepository.uow.Rollback();
                return BadRequest($"{nameof(Customer)} couldn't be inserted.");
            }
        }
        return CreatedAtAction(nameof(GetCustomerById), new { id = inserted.CustomerId }, inserted);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Customer> UpdateCustomer([FromRoute] int id, [FromBody] CustomerDto dto)
    {
        using(_customerRepository.uow) 
        {
            try 
            {
                _customerRepository.GetByCustomerId(id);
                Customer customer = Customer.CreateCustomer(dto);
                _customerRepository.Update(customer);
                _customerRepository.uow.Commit();
            }
            catch(ArgumentException)
            {
                return NotFound($"{nameof(Customer)} with {nameof(id)} {id} not found.");
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
            catch(Exception)
            {
                _customerRepository.uow.Rollback();
                return BadRequest($"{nameof(Customer)} couldn't be updated.");
            }
        }
        return NoContent();
    }
}
