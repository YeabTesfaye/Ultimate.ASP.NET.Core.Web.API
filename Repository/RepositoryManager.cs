
using Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<ICompanyRepository> _companyRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;
    public RepositoryManager(RepositoryContext repositoryContext){
        _repositoryContext = repositoryContext;
        _companyRepository = new Lazy<ICompanyRepository>(() => new 
        CompanyRepository(repositoryContext));
        _employeeRepository = new Lazy<IEmployeeRepository>(() => new
        EmployeeRepository(repositoryContext));
    }
    // returns the instance of `CompanyRepository` when accessed
    public ICompanyRepository Company => _companyRepository.Value;
    // returns the instance of `EmployeeRepository` when accesse  
    public IEmployeeRepository Employee => _employeeRepository.Value;

    public void Save() => _repositoryContext.SaveChanges();
}