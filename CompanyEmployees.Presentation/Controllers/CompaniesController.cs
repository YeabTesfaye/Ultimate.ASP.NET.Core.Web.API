using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    /// <summary>
    /// Handles CRUD operations for companies.
    /// </summary>
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompaniesController"/> class.
        /// </summary>
        /// <param name="service">The service manager for handling company-related operations.</param>
        public CompaniesController(IServiceManager service) => _service = service;

        /// <summary>
        /// Retrieves the list of all companies.
        /// </summary>
        /// <returns>List of companies.</returns>
        /// <response code="200">Returns the list of companies.</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
            return Ok(companies);
        }

        /// <summary>
        /// Retrieves a company by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the company.</param>
        /// <returns>The company details.</returns>
        /// <response code="200">Returns the company with the specified ID.</response>
        /// <response code="404">If the company with the specified ID is not found.</response>
        [HttpGet("{id:guid}", Name = "CompanyById")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetCompany([FromRoute] Guid id)
        {
            var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);
            return Ok(company);
        }

        /// <summary>
        /// Creates a new company.
        /// </summary>
        /// <param name="company">The company to be created.</param>
        /// <returns>The created company.</returns>
        /// <response code="201">Returns the newly created company.</response>
        /// <response code="400">If the company object is null.</response>
        /// <response code="422">If the model is invalid.</response>
        [HttpPost(Name = "CreateCompany")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
        }

        /// <summary>
        /// Retrieves a collection of companies by their unique identifiers.
        /// </summary>
        /// <param name="ids">The unique identifiers of the companies.</param>
        /// <returns>A list of companies with the specified IDs.</returns>
        /// <response code="200">Returns the companies with the specified IDs.</response>
        /// <response code="404">If no companies with the specified IDs are found.</response>
        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(
            BinderType = typeof(ArrayModelBinder)
        )] IEnumerable<Guid> ids)
        {
            var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);
            return Ok(companies);
        }

        /// <summary>
        /// Creates a collection of new companies.
        /// </summary>
        /// <param name="companyCollection">The collection of companies to be created.</param>
        /// <returns>The created companies.</returns>
        /// <response code="201">Returns the newly created companies.</response>
        /// <response code="400">If the company collection is null or empty.</response>
        [HttpPost("collection")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection == null || !companyCollection.Any())
            {
                return BadRequest("Company collection is null or empty.");
            }
            var (companies, ids) = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);
            return CreatedAtRoute("CompanyCollection", new { ids }, companies);
        }

        /// <summary>
        /// Deletes a company by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the company to be deleted.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the company was successfully deleted.</response>
        /// <response code="404">If the company with the specified ID is not found.</response>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
            return NoContent();
        }

        /// <summary>
        /// Updates an existing company.
        /// </summary>
        /// <param name="id">The unique identifier of the company to be updated.</param>
        /// <param name="company">The updated company details.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the company was successfully updated.</response>
        /// <response code="400">If the company object is null.</response>
        /// <response code="422">If the model is invalid.</response>
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
            return NoContent();
        }

        /// <summary>
        /// Provides options for the available HTTP methods.
        /// </summary>
        /// <returns>HTTP options.</returns>
        [HttpOptions]
        [Authorize]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Allow = "GET,OPTIONS,POST";
            return Ok();
        }
    }
}
