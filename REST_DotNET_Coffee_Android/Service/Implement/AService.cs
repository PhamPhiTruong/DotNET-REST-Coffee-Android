public abstract class AService<T>
{
    protected readonly ApplicationDbContext _context;

    protected readonly ILogger _logger;

    protected AService(ApplicationDbContext context, ILogger logger)
    {
        _context = context; _logger = logger;
    }
}
