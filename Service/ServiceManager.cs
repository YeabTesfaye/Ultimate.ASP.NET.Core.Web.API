using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger,
     IMapper mapper, IDataShaper<EmployeeDto> dataShaper, UserManager<User> userManager,
     IConfiguration configuration)
    {
        _companyService = new Lazy<ICompanyService>(() => new
        CompanyService(repositoryManager, logger, mapper));
        _employeeService = new Lazy<IEmployeeService>(() => new
        EmployeeService(repositoryManager, logger, mapper, dataShaper));
        _authenticationService = new Lazy<IAuthenticationService>(() =>
        new AuthenticationService(logger, mapper, userManager, configuration));
    }
    public ICompanyService CompanyService => _companyService.Value;
    public IEmployeeService EmployeeService => _employeeService.Value;

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}