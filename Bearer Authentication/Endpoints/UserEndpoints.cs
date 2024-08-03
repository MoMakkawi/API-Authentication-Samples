using Bearer_Authentication.Authentication.Models;
using Bearer_Authentication.Authentication.Services;

using Microsoft.AspNetCore.Mvc;

using Persistence.Contracts;

namespace Bearer_Authentication.Endpoints;
internal static class UserEndpoints
{
    private const string prefix = "/users";
    internal static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost(prefix + "/login", async ([FromBody] LoginRequest loginRequest, [FromServices] JwtAuthenticationService jwtAuthenticationService) =>
        {
            try
            {
                return await jwtAuthenticationService.LoginAsync(loginRequest);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }
        }).WithTags("Users")
        .AllowAnonymous();

        app.MapPost(prefix + "/login-user-info", async (HttpContext httpContext, IUserRepository userRepository) => 
        {
           var currentUser = await userRepository.GetLoginUserInfoAsync(httpContext);

            return new
            {
                currentUser.Id,
                currentUser.UserName,
                currentUser.Email,
                FullName = currentUser.FirstName + " " + currentUser.LastName,
            };

        }).WithTags("Users")
        .RequireAuthorization();
    }
}
