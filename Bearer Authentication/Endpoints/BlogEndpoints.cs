using System.Security.Claims;


using Microsoft.AspNetCore.Authorization;

using Persistence.Contracts;
using Persistence.Models;

internal static class BlogEndpoints
{
    internal static void MapBlogEndpoints(this WebApplication app)
    {
        // public endpoints => it work for unauthorized and authorized users 
        app.MapGet("/blogs", async (IBlogRepository blogRepository)
            => await blogRepository.GetAllAsync());

        app.MapGet("/blogs/{id}", async (int id, IBlogRepository blogRepository)
            => await blogRepository.GetByIdAsync(id) is Blog blog
            ? Results.Ok(blog) : Results.NotFound());

        // Not public endpoint => it work for only authorized user.
        app.MapPut("/blogs/{id}", [Authorize] async (int id, Blog inputBlog, IBlogRepository blogRepository) =>
        {
            var blog = await blogRepository.GetByIdAsync(id);
            if (blog is null) return Results.NotFound();

            await blogRepository.UpdateAsync(blog);

            return Results.NoContent();
        });

        app.MapDelete("/blogs/{id}", [Authorize] async (int id, IBlogRepository blogRepository) =>
        {
            await blogRepository.DeleteAsync(id);
            return Results.NoContent();
        });

        // Not public endpoint => it work for only authorized user, only for Admin.
        app.MapPost("/blogs", [Authorize] async (Blog blog, HttpContext httpContext, IBlogRepository blogRepository, IUserRepository UserRepository) =>
        {
            var userId = (httpContext.User.Identity as ClaimsIdentity)?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

            if (!int.TryParse(userId, out int id) 
            || await UserRepository.GetByIdAsync(id) is not { UserName: "Admin" })
                return Results.BadRequest("Sorry, Only admin can do this.");

            blog = await blogRepository.AddAsync(blog);
            return Results.Created($"/blogs/{blog.Id}", blog);
        });
    }
}
