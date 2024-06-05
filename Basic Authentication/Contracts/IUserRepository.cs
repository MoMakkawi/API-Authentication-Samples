using Basic_Authentication.Models;

namespace Basic_Authentication.Contracts;

internal interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByUserNameAsync(string userName);
}
