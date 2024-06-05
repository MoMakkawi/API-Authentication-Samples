using Basic_Authentication;

using Microsoft.EntityFrameworkCore;

internal static class Endpoints
{
    internal static void BlogEndpoints(this WebApplication app)
    {
        app.MapGet("/blogs", async (BloggingContext db) =>
        {
            return await db.Blogs.ToListAsync();
        });

        app.MapGet("/blogs/{id}", async (int id, BloggingContext db) =>
        {
            return await db.Blogs.FindAsync(id)
                is Blog blog
                    ? Results.Ok(blog)
                    : Results.NotFound();
        });

        app.MapPost("/blogs", async (Blog blog, BloggingContext db) =>
        {
            db.Blogs.Add(blog);
            await db.SaveChangesAsync();

            return Results.Created($"/blogs/{blog.Id}", blog);
        });

        app.MapPut("/blogs/{id}", async (int id, Blog inputBlog, BloggingContext db) =>
        {
            var blog = await db.Blogs.FindAsync(id);

            if (blog is null)
            {
                return Results.NotFound();
            }

            blog.Name = inputBlog.Name;
            blog.Description = inputBlog.Description;
            blog.Url = inputBlog.Url;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/blogs/{id}", async (int id, BloggingContext db) =>
        {
            var blog = await db.Blogs.FindAsync(id);

            if (blog is not null)
            {
                db.Blogs.Remove(blog);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }

            return Results.NotFound();
        });
    }
}
