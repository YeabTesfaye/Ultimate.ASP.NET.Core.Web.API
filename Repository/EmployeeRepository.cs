

using Contracts;

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

    public Employee? GetEmployee(Guid companyId, Guid id, bool trackChanges) => 
 FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), 
   trackChanges)
 .SingleOrDefault();

    public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)
    {
        return [.. FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).OrderBy(e => e.Name)];
    }
}