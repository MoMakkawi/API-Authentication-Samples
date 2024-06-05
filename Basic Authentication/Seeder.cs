using Basic_Authentication;

public static class Seeder  
{
    public static void SeedData(this BloggingContext context)
    {
        if (context.Blogs.Any()) return;

        context.Blogs.AddRange(
                        new Blog { Name = "Tech Blog", Description = "A blog about the latest in tech", Url = "https://techblog.com" },
                        new Blog { Name = "Food Blog", Description = "Delicious recipes and cooking tips", Url = "https://foodblog.com" },
                        new Blog { Name = "Travel Blog", Description = "Travel tips and destination guides", Url = "https://travelblog.com" }
                    );

        context.SaveChanges();
    }
}
