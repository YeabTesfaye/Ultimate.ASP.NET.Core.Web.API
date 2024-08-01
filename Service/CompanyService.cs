using Contracts;
using Entities.Models;
using Service.Contracts;

namespace Service;

internal  sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    public CompanyService(IRepositoryManager repository){
        _repository = repository;
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var companies = _repository.Company.GetAllCompanies(trackChanges);
            return companies;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}