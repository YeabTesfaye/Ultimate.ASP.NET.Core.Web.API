using Contracts;
using Service.Contracts;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        public EmployeeService(IRepositoryManager repository)
        {
            _repository = repository;
        }
    }
}