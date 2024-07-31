using Contracts;
using Service.Contracts;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    public ServiceManager(IRepositoryManager repositoryManager)
    {
        _companyService = new Lazy<ICompanyService>(() => new
        CompanyService(repositoryManager));
        _employeeService = new Lazy<IEmployeeService>(() => new
        EmployeeService(repositoryManager));
    }
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;
}