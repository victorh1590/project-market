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
public class ProjectAdvertisementController(
    ProjectAdvertisementRepository projectAdvertisementRepository,
    ProjectAdvertisementFactory projectAdvertisementFactory) : ControllerBase
{
    [HttpGet("{id:int}")]
    public ActionResult<ProjectAdvertisement> GetProjectAdvertisementById(int id)
    {
        try
        {
            return Ok(projectAdvertisementRepository.GetProjectAdvertisementById(id));
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
        using(projectAdvertisementRepository.UnitOfWork) 
        {
            try
            {
                var projectAdvertisement = projectAdvertisementFactory.CreateProjectAdvertisement(dto);
                inserted = projectAdvertisementRepository.Insert(projectAdvertisement);
                projectAdvertisementRepository.UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                projectAdvertisementRepository.UnitOfWork.Rollback();
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
        using(projectAdvertisementRepository.UnitOfWork) 
        {
            try
            {
                projectAdvertisementRepository.GetProjectAdvertisementById(id);
                var projectAdvertisement = projectAdvertisementFactory.CreateProjectAdvertisement(dto);
                projectAdvertisementRepository.Update(projectAdvertisement);
                projectAdvertisementRepository.UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                projectAdvertisementRepository.UnitOfWork.Rollback();
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