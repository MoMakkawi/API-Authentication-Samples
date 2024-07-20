using Bearer_Authentication.Models;

namespace Bearer_Authentication.Contracts;

internal interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByUserNameAsync(string userName);
}
