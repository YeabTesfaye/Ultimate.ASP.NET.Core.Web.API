using System.Linq;
using System.Text.Json;
using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace CompanyEmployees.Presentation.Controllers
{
    /// <summary>
    /// Handles CRUD operations for employees within a specific company.
    /// </summary>
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController"/> class.
        /// </summary>
        /// <param name="service">The service manager for handling employee-related operations.</param>
        public EmployeesController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves a list of employees for a specific company.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company.</param>
        /// <param name="employeeParameters">The parameters for employee query (pagination, filtering, etc.).</param>
        /// <returns>A paginated list of employees for the specified company.</returns>
        /// <response code="200">Returns the list of employees for the company.</response>
        [HttpGet]
        [HttpHead]
        [Authorize]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,
            [FromQuery] EmployeeParameters employeeParameters)
        {
            var pagedResult = await _service.EmployeeService.GetEmployeesAsync(companyId,
                employeeParameters, trackChanges: false);
            // Set the pagination metadata header
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            return Ok(pagedResult.employees);
        }

        /// <summary>
        /// Retrieves an employee by their unique identifier within a specific company.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company.</param>
        /// <param name="id">The unique identifier of the employee.</param>
        /// <returns>The details of the employee.</returns>
        /// <response code="200">Returns the employee details.</response>
        /// <response code="404">If the employee with the specified ID is not found.</response>
        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee = await _service.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);
            return Ok(employee);
        }

        /// <summary>
        /// Creates a new employee for a specific company.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company.</param>
        /// <param name="employee">The employee data to be created.</param>
        /// <returns>The newly created employee.</returns>
        /// <response code="201">Returns the created employee.</response>
        /// <response code="400">If the employee object is null.</response>
        /// <response code="422">If the model is invalid.</response>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            var employeeToReturn = await _service.EmployeeService
                .CreateEmployeeForCompanyAsync(companyId, employee, trackChanges: false);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }

        /// <summary>
        /// Deletes an employee from a specific company.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company.</param>
        /// <param name="id">The unique identifier of the employee to be deleted.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the employee was successfully deleted.</response>
        /// <response code="404">If the employee with the specified ID is not found.</response>
        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            await _service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, trackChanges: false);
            return NoContent();
        }

        /// <summary>
        /// Updates an existing employee in a specific company.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company.</param>
        /// <param name="id">The unique identifier of the employee to be updated.</param>
        /// <param name="employee">The updated employee data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the employee was successfully updated.</response>
        /// <response code="400">If the employee object is null.</response>
        /// <response code="422">If the model is invalid.</response>
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
            [FromBody] EmployeeForUpdateDto employee)
        {
            await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee,
                compTrackChanges: false, empTrackChanges: true);
            return NoContent();
        }

        /// <summary>
        /// Partially updates an existing employee in a specific company using JSON Patch.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company.</param>
        /// <param name="id">The unique identifier of the employee to be partially updated.</param>
        /// <param name="patchDoc">The JSON Patch document containing the updates.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the employee was successfully partially updated.</response>
        /// <response code="400">If the patch document is null.</response>
        /// <response code="422">If the model is invalid after applying the patch.</response>
        [HttpPatch("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
            [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest("patchDoc object sent from client is null.");
            }
            var (employeeToPatch, employeeEntity) = await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, id, compTrackChanges: false, empTrackChanges: true);
            // Apply the patch document to the DTO
            patchDoc.ApplyTo(employeeToPatch);
            //TryValidateModel used to manually trigger model validation for a given model 
            TryValidateModel(employeeToPatch);
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            await _service.EmployeeService.SaveChangesForPatchAsync(employeeToPatch, employeeEntity);
            return NoContent();
        }
    }
}
