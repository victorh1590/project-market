using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.Factory;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Application.Controller;

[ApiController]
[Route("[controller]")]
public class PaymentOfferController(IUnitOfWork uow) : ControllerBase
{
    private readonly PaymentOfferRepository _paymentOfferRepository = new(uow);

    [HttpGet("{id:int}")]
    public ActionResult<PaymentOffer> GetPaymentOfferById(int id)
    {
        try
        {
            return Ok(_paymentOfferRepository.GetByPaymentOfferById(id));
        }
        catch (Exception e)
        {
            return e switch
            {
                ArgumentException => NotFound($"{nameof(PaymentOffer)} with {nameof(id)} {id} not found."),
                _ => BadRequest($"{nameof(PaymentOffer)} couldn't be retrieved.")
            };
        }
    }

    [HttpPost]
    public ActionResult<PaymentOffer> PostPaymentOffer([FromBody] PaymentOfferDto dto)
    {
        PaymentOffer inserted;
        using(_paymentOfferRepository.uow) 
        {
            try
            {
                PaymentOfferFactory factory = new();
                PaymentOffer PaymentOffer = factory.CreatePaymentOffer(dto);
                inserted = _paymentOfferRepository.Insert(PaymentOffer);
                _paymentOfferRepository.uow.Commit();
            }
            catch (Exception e)
            {
                _paymentOfferRepository.uow.Rollback();
                return (e) switch
                {
                    ValidationException => BadRequest("Body is in a invalid state."),
                    SqlException => BadRequest("Error saving PaymentOffer."),
                    _ => BadRequest($"{nameof(PaymentOffer)} couldn't be inserted.")
                };
            }
        }
        return CreatedAtAction(nameof(GetPaymentOfferById), new { id = inserted.PaymentOfferId }, inserted);
    }

    [HttpPut("{id:int}")]
    public ActionResult<PaymentOffer> UpdatePaymentOffer([FromRoute] int id, [FromBody] PaymentOfferDto dto)
    {
        using(_paymentOfferRepository.uow) 
        {
            try
            {
                _paymentOfferRepository.GetByPaymentOfferById(id);
                PaymentOfferFactory factory = new(uow);
                PaymentOffer PaymentOffer = factory.CreatePaymentOffer(dto);
                _paymentOfferRepository.Update(PaymentOffer);
                _paymentOfferRepository.uow.Commit();
            }
            catch (Exception e)
            {
                _paymentOfferRepository.uow.Rollback();
                return e switch
                {
                    ArgumentException => NotFound($"{nameof(PaymentOffer)} with {nameof(id)} {id} not found."),
                    ValidationException => BadRequest("Body is on a invalid state."),
                    SqlException => BadRequest("Error updating PaymentOffer."),
                    _ => BadRequest($"{nameof(PaymentOffer)} couldn't be updated.")
                };
            }
        }
        return NoContent();
    }
}