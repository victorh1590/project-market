using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.Factory;
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
        catch (Exception e)
        {
            return e switch
            {
                ArgumentException => NotFound($"{nameof(Customer)} with {nameof(id)} {id} not found."),
                _ => BadRequest($"{nameof(Customer)} couldn't be retrieved.")
            };
        }
    }

    [HttpPost]
    public ActionResult<Customer> PostCustomer([FromBody] CustomerDto dto)
    {
        Customer inserted;
        using(_customerRepository.UnitOfWork) 
        {
            try
            {
                CustomerFactory factory = new();
                Customer customer = factory.CreateCustomer(dto);
                inserted = _customerRepository.Insert(customer);
                _customerRepository.UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                _customerRepository.UnitOfWork.Rollback();
                return (e) switch
                {
                    ValidationException => BadRequest("Body is in a invalid state."),
                    SqlException => BadRequest("Error saving customer."),
                    _ => BadRequest($"{nameof(Customer)} couldn't be inserted.")
                };
            }
        }
        return CreatedAtAction(nameof(GetCustomerById), new { id = inserted.CustomerId }, inserted);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Customer> UpdateCustomer([FromRoute] int id, [FromBody] CustomerDto dto)
    {
        using(_customerRepository.UnitOfWork) 
        {
            try
            {
                _customerRepository.GetByCustomerId(id);
                CustomerFactory factory = new();
                Customer customer = factory.CreateCustomer(dto);
                _customerRepository.Update(customer);
                _customerRepository.UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                _customerRepository.UnitOfWork.Rollback();
                return e switch
                {
                    ArgumentException => NotFound($"{nameof(Customer)} with {nameof(id)} {id} not found."),
                    ValidationException => BadRequest("Body is on a invalid state."),
                    SqlException => BadRequest("Error updating customer."),
                    _ => BadRequest($"{nameof(Customer)} couldn't be updated.")
                };
            }
        }
        return NoContent();
    }
}
