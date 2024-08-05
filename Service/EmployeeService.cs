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

    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
    {
        _ = _repository.Company.GetCompany(companyId, trackChanges: false) ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);
        _repository.Employee.CreateEmployeeForCompany(companyId,employeeEntity);
        _repository.Save();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeToReturn;
        }

    public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
    {
        _ = _repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(id);
        var employeeForCompany = _repository.Employee.GetEmployee(companyId,id,trackChanges) ?? throw new EmployeeNotFoundException(id);
        
        _repository.Employee.DeleteEmployee(employeeForCompany);
        _repository.Save();
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        _ = _repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeDb = _repository.Employee.GetEmployee(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
        var employee = _mapper.Map<EmployeeDto>(employeeDb);
        return employee;
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        _ = _repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeesFromDb = _repository.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
        return employeesDto;
    }

    public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        _ = _repository.Company.GetCompany(companyId, compTrackChanges)
         ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = _repository.Employee.GetEmployee(companyId,id,empTrackChanges) 
        ?? throw new EmployeeNotFoundException(companyId);
        _mapper.Map(employeeForUpdate, employeeEntity);
        _repository.Save();

    }
}