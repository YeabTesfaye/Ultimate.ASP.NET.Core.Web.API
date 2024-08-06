

using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee) => Delete(employee);

    

    public Task<Employee?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) => 
    FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), 
      trackChanges)
    .SingleOrDefaultAsync();


    public async Task<List<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges) =>
         await FindByCondition(e => e.CompanyId.Equals(companyId),trackChanges).OrderBy(e => e.Name).ToListAsync();
    
}