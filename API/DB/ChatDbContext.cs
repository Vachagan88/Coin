using Core.Entities;
using DB.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class ChatDbContext : DbContext
{
    private readonly string _connectionString;

    public ChatDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new ResourceConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Resource> Resources { get; set; }
    public DbSet<User> Users { get; set; }
}
