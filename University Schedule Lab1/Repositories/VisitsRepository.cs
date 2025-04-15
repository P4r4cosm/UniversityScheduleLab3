using University_Schedule_Lab1.Data;

namespace University_Schedule_Lab1.Repositories;

public class VisitsRepository
{
    private readonly ApplicationContext _context;
    private readonly ILogger<VisitsRepository> _logger;

    public VisitsRepository(ApplicationContext context, ILogger<VisitsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
}