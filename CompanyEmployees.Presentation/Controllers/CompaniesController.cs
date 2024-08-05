
using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;
    public CompaniesController(IServiceManager service) => _service = service;
    [HttpGet]
    public IActionResult GetCompanies()
    {
        var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
        return Ok(companies);

    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany([FromRoute] Guid id)
    {

        var company = _service.CompanyService.GetCompany(id, trackChanges: false);
        return Ok(company);
    }
    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
    {
        
        if (company is null)
        {
            return BadRequest("CompanyForCreationDto object is null");
        }
        if(!ModelState.IsValid){
            return UnprocessableEntity(ModelState);
        }
        var createdCompany = _service.CompanyService.CreateCompany(company);
        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpGet("collection/({ids})", Name ="CompanyCollection")]
    public IActionResult GetCompanyCollection([ModelBinder(
        BinderType =typeof(ArrayModelBinder)
    )] IEnumerable<Guid> ids){
        
        var companys = _service.CompanyService.GetByIds(ids, trackChanges:false);
        return Ok(companys);
    }
    [HttpPost("collection")]
    public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companycollecion){
        var (companies, ids) = _service.CompanyService.CreateCompanyCollection(companycollecion);
        return CreatedAtRoute("CompanyCollection",new {ids}, companies);
    }
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteCompany(Guid id){
        _service.CompanyService.DeleteCompany(id,trackChanges:false);
        return NoContent();
    }
    [HttpPut("{id:guid}")]
    public IActionResult UpdateCompany(Guid id,[FromBody] CompanyForUpdateDto company){
       
        if(company is null)
               return BadRequest("CompanyForUpdateDto object is null");
        if(!ModelState.IsValid){
            return UnprocessableEntity(ModelState);
        }
        _service.CompanyService.UpdateCompany(id,company,trackChanges:true);
        return NoContent();
    }
}