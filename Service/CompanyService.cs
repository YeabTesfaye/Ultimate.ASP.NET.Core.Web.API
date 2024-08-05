using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Entities.Models.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;


namespace Service;

internal  sealed class CompanyService : ICompanyService
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    public CompanyService(IRepositoryManager repository, ILoggerManager logger,  IMapper mapper){
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public CompanyDto CreateCompany(CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);
        _repository.Company.CreateCompany(companyEntity);
        _repository.Save();

        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
        return companyToReturn;
    }

    public (IEnumerable<CompanyDto> companies, string ids) 
    CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if(companyCollection is null){
            throw new CompanyCollectionBadRequest();
        }
        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach(var company in companyEntities){
            _repository.Company.CreateCompany(company);
        }
        _repository.Save();
        var companyCollectionToReturn = 
        _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
        return (companies:companyCollectionToReturn, ids);
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
      
            var companies = _repository.Company.GetAllCompanies(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
        
    }

    public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
    {
       
       if(ids is null){
        throw new IdParametersBadRequestException();
       }
       var companyEntities = _repository.Company.GetByIds(ids,trackChanges);
       //Verifies that the number of entities retrieved matches the number of IDs requested
       if(ids.Count() != companyEntities.Count()){
        throw new CollectionByIdsBadRequestException();
       }
       var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
       return companiesToReturn;
    }

    public CompanyDto GetCompany(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null) throw new CompanyNotFoundException(companyId);
        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }
}