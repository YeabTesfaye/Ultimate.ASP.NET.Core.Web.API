using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;
using Shared.RequestFeatures;
namespace Contracts;

public interface IEmployeeRepository
{
    // Task<List<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    Task<Employee?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployee(Employee employee);
    Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, 
    EmployeeParameters employeeParameters, bool trackChanges);
    
}
