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
public class ProjectAdvertisementController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly ProjectAdvertisementRepository _projectAdvertisementRepository = new(unitOfWork);

    [HttpGet("{id:int}")]
    public ActionResult<ProjectAdvertisement> GetProjectAdvertisementById(int id)
    {
        try
        {
            return Ok(_projectAdvertisementRepository.GetProjectAdvertisementById(id));
        }
        catch (Exception e)
        {
            return e switch
            {
                ArgumentException => NotFound($"{nameof(ProjectAdvertisement)} with {nameof(id)} {id} not found."),
                _ => BadRequest($"{nameof(ProjectAdvertisement)} couldn't be retrieved.")
            };
        }
    }

    [HttpPost]
    public ActionResult<ProjectAdvertisement> PostProjectAdvertisement([FromBody] ProjectAdvertisementDto dto)
    {
        ProjectAdvertisement inserted;
        using(_projectAdvertisementRepository.UnitOfWork) 
        {
            try
            {
                ProjectAdvertisementFactory factory = new(unitOfWork);
                ProjectAdvertisement projectAdvertisement = factory.CreateProjectAdvertisement(dto);
                inserted = _projectAdvertisementRepository.Insert(projectAdvertisement);
                _projectAdvertisementRepository.UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                _projectAdvertisementRepository.UnitOfWork.Rollback();
                return (e) switch
                {
                    ValidationException => BadRequest("Body is in a invalid state."),
                    SqlException => BadRequest("Error saving ProjectAdvertisement."),
                    _ => BadRequest($"{nameof(ProjectAdvertisement)} couldn't be inserted.")
                };
            }
        }
        return CreatedAtAction(nameof(GetProjectAdvertisementById), new { id = inserted.ProjectAdvertisementId }, inserted);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProjectAdvertisement> UpdateProjectAdvertisement([FromRoute] int id, [FromBody] ProjectAdvertisementDto dto)
    {
        using(_projectAdvertisementRepository.UnitOfWork) 
        {
            try
            {
                _projectAdvertisementRepository.GetProjectAdvertisementById(id);
                ProjectAdvertisementFactory factory = new(unitOfWork);
                ProjectAdvertisement paymentOffer = factory.CreateProjectAdvertisement(dto);
                _projectAdvertisementRepository.Update(paymentOffer);
                _projectAdvertisementRepository.UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                _projectAdvertisementRepository.UnitOfWork.Rollback();
                return e switch
                {
                    ArgumentException => NotFound($"{nameof(ProjectAdvertisement)} with {nameof(id)} {id} not found."),
                    ValidationException => BadRequest("Body is on a invalid state."),
                    SqlException => BadRequest("Error updating ProjectAdvertisement."),
                    _ => BadRequest($"{nameof(ProjectAdvertisement)} couldn't be updated.")
                };
            }
        }
        return NoContent();
    }
}