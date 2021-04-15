using System.Collections.Generic;
using System.Threading.Tasks;
using Commands.Application.Models;
using Commands.Domain.Entities;

namespace Commands.Domain.Services
{
    public interface ICommandServices
    {
        
        Task<IEnumerable<CommandEntity>> GetAllCommandsAsync();
        
        Task<CommandEntity> CreateCommandAsync(CommandCreate command);
        
        Task<bool> SaveChangesAsync();

        Task<CommandEntity> GetCommandById(int id);

        Task UpdateCommand(CommandUpdate command, int id);

        Task DeleteCommandById(CommandEntity command);
    }
}