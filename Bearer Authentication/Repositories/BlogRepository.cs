using Bearer_Authentication.Contracts;
using Bearer_Authentication.Data;
using Bearer_Authentication.Models;

namespace Bearer_Authentication.Repositories;

public class BlogRepository(BloggingContext dbContext) 
    :BaseRepository<Blog>(dbContext), IBlogRepository;