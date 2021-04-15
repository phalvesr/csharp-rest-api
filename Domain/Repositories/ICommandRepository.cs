using System.Collections.Generic;
using System.Threading.Tasks;
using Commands.Application.Models;
using Commands.Domain.Entities;

namespace Commands.Domain.Repositories
{
    public interface ICommandRepository
    {
        
        Task<IEnumerable<CommandEntity>> GetAllCommandsAsync();
        
        Task CreateCommandAsync(CommandEntity command);        
        
        Task<bool> SaveChangesAsync();
        
        Task<CommandEntity> GetCommandById(int id);

        Task UpdateCommand(CommandEntity command);
        
        Task DeleteCommandById(CommandEntity command);
    }
}