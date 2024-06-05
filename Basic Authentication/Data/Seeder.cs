﻿using Basic_Authentication.Data;
using Basic_Authentication.Models;

public static class Seeder
{
    public static BloggingContext SeedBlogs(this BloggingContext context)
    {
        if (!context.Blogs.Any())
        {
            context.Blogs.AddRange(
                new Blog { Name = "Tech Blog", Description = "A blog about the latest in tech", Url = "https://techblog.com" },
                new Blog { Name = "Food Blog", Description = "Delicious recipes and cooking tips", Url = "https://foodblog.com" },
                new Blog { Name = "Travel Blog", Description = "Travel tips and destination guides", Url = "https://travelblog.com" }
            );

            context.SaveChanges();
        }

        return context;
    }
    public static BloggingContext SeedUsers(this BloggingContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User { UserName = "Admin", Password = "Password" },
                new User { UserName = "MoMakkawi", Password = "mohamad123" }
            );

            context.SaveChanges();
        }

        return context;
    }
}
