namespace Entities.Models.Exceptions
{
    public class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException(Guid companyId) : 
        base($"The Company with id : {companyId} doesn't exist in the database")
        {

        }
    }
}