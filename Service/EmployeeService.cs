using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
namespace Service;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);
        var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);
        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repository.SaveAsync();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);
        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);
        _repository.Employee.DeleteEmployee(employeeDb);
        await _repository.SaveAsync();
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);
        var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges)
         ?? throw new EmployeeNotFoundException(id);
        var employee = _mapper.Map<EmployeeDto>(employeeDb);
        return employee;
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id,
    bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);
        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);
        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeDb);
        return (employeeToPatch, employeeEntity: employeeDb);
    }

    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)>
 GetEmployeesAsync
 (Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        if(!employeeParameters.ValidAgeRange)
              throw new MaxAgeRangeBadRequestException();
        await CheckIfCompanyExists(companyId, trackChanges);
        var employeesWithMetaData = await _repository.Employee
        .GetEmployeesAsync(companyId, employeeParameters, trackChanges);
        var employeesDto =
        _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
        return (employees: employeesDto, metaData: employeesWithMetaData.MetaData);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);
        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);
        _mapper.Map(employeeForUpdate, employeeDb);
        await _repository.SaveAsync();

    }
    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
    {
        _ = await _repository.Company.GetCompanyAsync(companyId, trackChanges)
        ?? throw new CompanyNotFoundException(companyId);
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(
        Guid companyId, Guid id, bool trackChanges)
    {
        var employeeDb = await _repository.Employee.GetEmployeeAsync(
            companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
        return employeeDb;
    }


}