using Basic_Authentication.Contracts;
using Basic_Authentication.Data;
using Basic_Authentication.Models;

using Microsoft.EntityFrameworkCore;

namespace Basic_Authentication.Repositories;

internal sealed class UserRepository(BloggingContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> FindByUserNameAsync(string userName)
        => await _dbContext.Users.FirstOrDefaultAsync(u => string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase));
}
