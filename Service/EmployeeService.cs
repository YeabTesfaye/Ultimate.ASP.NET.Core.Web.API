using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

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
        _ = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false) ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);
        _repository.Employee.CreateEmployeeForCompany(companyId,employeeEntity);
       await _repository.SaveAsync();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeToReturn;
        }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
        _ = await _repository.Company.GetCompanyAsync(companyId, trackChanges) 
        ?? throw new CompanyNotFoundException(id);
        var employeeForCompany = await _repository.Employee.GetEmployeeAsync(companyId,id,trackChanges) 
        ?? throw new EmployeeNotFoundException(id);
        
        _repository.Employee.DeleteEmployee(employeeForCompany);
        await _repository.SaveAsync();
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        _ = await _repository.Company.GetCompanyAsync(companyId, trackChanges) 
        ?? throw new CompanyNotFoundException(companyId);
        var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges)
         ?? throw new EmployeeNotFoundException(id);
        var employee = _mapper.Map<EmployeeDto>(employeeDb);
        return employee;
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, 
    bool compTrackChanges, bool empTrackChanges)
    {
        _ = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges)
        ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, 
                     empTrackChanges) ?? throw new EmployeeNotFoundException(companyId);
       var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
       return (employeeToPatch,employeeEntity);
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        _ = await _repository.Company.GetCompanyAsync(companyId, trackChanges)
         ?? throw new CompanyNotFoundException(companyId);
        var employeesFromDb =await _repository.Employee.GetEmployeesAsync(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
        return employeesDto;
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch,employeeEntity);
       await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        _ =await _repository.Company.GetCompanyAsync(companyId, compTrackChanges)
         ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId,id,empTrackChanges) 
        ?? throw new EmployeeNotFoundException(companyId);
        _mapper.Map(employeeForUpdate, employeeEntity);
       await _repository.SaveAsync();

    }
    
}