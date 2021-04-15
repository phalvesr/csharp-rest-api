using Commands.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Commands.Infrastructure.DataProviders.Databases
{
    public class CommandContext : DbContext
    {
        public CommandContext(DbContextOptions<CommandContext> options): base(options)
        {  }

        public DbSet<CommandEntity> Commands { get; set; }
    }
}