using Microsoft.EntityFrameworkCore;

namespace API_ClinicaMedica.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
}