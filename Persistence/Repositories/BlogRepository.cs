using Persistence.Contracts;
using Persistence.Data;
using Persistence.Models;

namespace Persistence.Repositories;

public class BlogRepository(BloggingContext dbContext)
    : BaseRepository<Blog>(dbContext), IBlogRepository;