using Microsoft.EntityFrameworkCore;

namespace HealthFlow.Data.Contexts;

public class HealthFlowDbContext : DbContext
{
    public HealthFlowDbContext(
        DbContextOptions<HealthFlowDbContext> options)
        : base(options)
    {
    }
}
