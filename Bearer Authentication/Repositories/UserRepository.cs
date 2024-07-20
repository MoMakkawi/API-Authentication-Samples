using Bearer_Authentication.Contracts;
using Bearer_Authentication.Data;
using Bearer_Authentication.Models;
using Bearer_Authentication.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Basic_Authentication.Repositories;

internal sealed class UserRepository(BloggingContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> FindByUserNameAsync(string userName)
        => await _dbContext.Users.FirstOrDefaultAsync(u => string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase));
}
