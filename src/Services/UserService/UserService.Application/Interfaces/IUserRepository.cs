using UserService.Domain.Aggregates;

namespace UserService.Application.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
    }
}
