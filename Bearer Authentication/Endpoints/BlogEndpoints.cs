using Microsoft.AspNetCore.Authorization;

using Persistence.Contracts;
using Persistence.Models;

internal static class BlogEndpoints
{
    private const string prefix = "/blogs";
    internal static void MapBlogEndpoints(this WebApplication app)
    {
        // public endpoints => it work for unauthorized and authorized users 
        app.MapGet(prefix, async (IBlogRepository blogRepository)
            => await blogRepository.GetAllAsync())
            .WithTags("Blogs");

        app.MapGet(prefix + "/{id}", async (int id, IBlogRepository blogRepository)
            => await blogRepository.GetByIdAsync(id) is Blog blog ? Results.Ok(blog) : Results.NotFound())
            .WithTags("Blogs");

        // Not public endpoint => it work for only authorized user.
        app.MapPut(prefix + "/{id}", [Authorize] async (int id, Blog inputBlog, IBlogRepository blogRepository) =>
        {
            var blog = await blogRepository.GetByIdAsync(id);
            if (blog is null) return Results.NotFound();

            await blogRepository.UpdateAsync(blog);

            return Results.NoContent();
        }).WithTags("Blogs");

        app.MapDelete(prefix + "/{id}", [Authorize] async (int id, IBlogRepository blogRepository) =>
        {
            await blogRepository.DeleteAsync(id);
            return Results.NoContent();
        }).WithTags("Blogs");

        // Not public endpoint => it work for only authorized user, only for Admin.
        app.MapPost(prefix, [Authorize] async (Blog blog, HttpContext httpContext, IBlogRepository blogRepository, IUserRepository UserRepository) =>
        {
            var currentUser = await UserRepository.GetLoginUserInfoAsync(httpContext);

            if (currentUser is not { UserName: "Admin" })
                return Results.BadRequest("Sorry, Only admin can do this.");

            blog = await blogRepository.AddAsync(blog);
            return Results.Created($"/blogs/{blog.Id}", blog);
        }).WithTags("Blogs");
    }
}
