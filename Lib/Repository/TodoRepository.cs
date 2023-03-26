using Lib.Model.Todo;
using Microsoft.EntityFrameworkCore;

namespace Lib.Repository
{
  public class TodoRepository : DbContext
  {
    public TodoRepository(DbContextOptions<TodoRepository> options)
    : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
  }
}
