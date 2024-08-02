namespace Entities.Exceptions;

public sealed class IdParametersBadRequestException:BadRequestException
{
    public IdParametersBadRequestException():base("Parameter ids is null")
    {
        
    }
}

public sealed class CollectionByIdsBadRequestException : BadRequestException
{
    public CollectionByIdsBadRequestException() : base("CollectionByIdsBadRequestException")
    {
    }
}