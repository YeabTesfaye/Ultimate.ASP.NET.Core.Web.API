using Contracts;
using Service.Contracts;

namespace Service;

internal  sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    public CompanyService(IRepositoryManager repository){
        _repository = repository;
    }
}