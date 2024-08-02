using Contracts;
using Entities.Models;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>,ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public void CreateCompany(Company company)
    {
        Create(company);
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        [.. FindAll(trackChanges).OrderBy(c => c.Name)];

    public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
    {
        return [.. FindByCondition(x =>  ids.Contains(x.Id),trackChanges)];
    }

    public Company? GetCompany(Guid companyId, bool trackChanges)
    {
        return FindByCondition(c => c.Id.Equals(companyId), trackChanges)
        .SingleOrDefault();
    }
}