using UserService.Application.Interfaces;
using UserService.Domain.Aggregates;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(UserDbContext context) : base(context)
        {
        }
    }
}
