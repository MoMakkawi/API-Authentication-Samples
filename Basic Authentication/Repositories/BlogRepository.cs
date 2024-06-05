using Basic_Authentication.Contracts;
using Basic_Authentication.Data;

namespace Basic_Authentication.Repositories;

public class BlogRepository(BloggingContext dbContext) 
    :BaseRepository<Blog>(dbContext), IBlogRepository;