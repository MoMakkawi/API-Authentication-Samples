using Persistence.Models;

namespace Persistence.Contracts;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByUserNameAsync(string userName);
}
