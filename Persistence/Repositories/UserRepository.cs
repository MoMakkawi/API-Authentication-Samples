using Persistence.Contracts;
using Persistence.Data;
using Persistence.Models;

using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Basic_Authentication.Repositories;

public sealed class UserRepository(BloggingContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> FindByUserNameAsync(string userName)
        => await _dbContext.Users.FirstOrDefaultAsync(u => string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase));

    public async Task<User> GetLoginUserInfoAsync(HttpContext httpContext)
    {
        var userId = (httpContext.User.Identity as ClaimsIdentity)?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

        if (!int.TryParse(userId, out int id))
            throw new ArgumentException($"{nameof(userId)} must be an integer.");
       
        return await GetByIdAsync(id);
    }
}
