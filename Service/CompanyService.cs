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

    public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);
        _repository.Company.CreateCompany(companyEntity);
       await _repository.SaveAsync();

        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
        return companyToReturn;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)>
    CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if(companyCollection is null){
            throw new CompanyCollectionBadRequest();
        }
        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach(var company in companyEntities){
            _repository.Company.CreateCompany(company);
        }
      await  _repository.SaveAsync();
        var companyCollectionToReturn = 
        _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
        return (companies:companyCollectionToReturn, ids);
    }


    public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, 
        trackChanges) ?? throw new CompanyNotFoundException(companyId);
        _repository.Company.DeleteCompany(company);
        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
    {
      
            var companies = await _repository.Company.GetAllCompaniesAsyc(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
        
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
       
       if(ids is null)
        throw new IdParametersBadRequestException();
       
       var companyEntities = await _repository.Company.GetByIdsAsync(ids,trackChanges);
       //Verifies that the number of entities retrieved matches the number of IDs requested
       if(ids.Count() != companyEntities.Count()){
        throw new CollectionByIdsBadRequestException();
       }
       var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
       return companiesToReturn;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges)
         ?? throw new CompanyNotFoundException(companyId);
         
        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
    {
        var companyEntity = await _repository.Company.GetCompanyAsync(companyId,trackChanges) 
        ?? throw new CompanyNotFoundException(companyId);
        _mapper.Map(companyForUpdate,companyEntity);
       await _repository.SaveAsync();
    }

    
}