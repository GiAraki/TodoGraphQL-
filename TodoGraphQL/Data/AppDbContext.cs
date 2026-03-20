using Microsoft.EntityFrameworkCore;
using TodoGraphQL.Models;

namespace TodoGraphQL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // Cada DbSet representa uma tabela no banco
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<User> Users => Set<User>();
}