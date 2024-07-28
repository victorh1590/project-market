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
public class PaymentOfferController(
    PaymentOfferRepository paymentOfferRepository, 
    PaymentOfferFactory paymentOfferFactory) : ControllerBase
{
    [HttpGet("{id:int}")]
    public ActionResult<PaymentOffer> GetPaymentOfferById(int id)
    {
        try
        {
            return Ok(paymentOfferRepository.GetPaymentOfferById(id));
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
        using(paymentOfferRepository.UnitOfWork) 
        {
            try
            {
                PaymentOffer paymentOffer = paymentOfferFactory.CreatePaymentOffer(dto);
                inserted = paymentOfferRepository.Insert(paymentOffer);
                paymentOfferRepository.UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                paymentOfferRepository.UnitOfWork.Rollback();
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
        using(paymentOfferRepository.UnitOfWork) 
        {
            try
            {
                paymentOfferRepository.GetPaymentOfferById(id);
                PaymentOffer paymentOffer = paymentOfferFactory.CreatePaymentOffer(dto);
                paymentOfferRepository.Update(paymentOffer);
                paymentOfferRepository.UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                paymentOfferRepository.UnitOfWork.Rollback();
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