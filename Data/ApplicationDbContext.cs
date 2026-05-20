using Microsoft.EntityFrameworkCore;
using WebApp.web.models;

namespace WebApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<Brand> Brand { get; set; }

}