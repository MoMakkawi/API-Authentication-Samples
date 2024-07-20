using Persistence.Contracts;
using Persistence.Data;
using Persistence.Models;

using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;

namespace Basic_Authentication.Repositories;

public sealed class UserRepository(BloggingContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> FindByUserNameAsync(string userName)
        => await _dbContext.Users.FirstOrDefaultAsync(u => string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase));
}
