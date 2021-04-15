using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Commands.Application.Models;
using Commands.Domain.Entities;
using Commands.Domain.Repositories;
using Commands.Domain.Services;

namespace Commands.Application.Services
{
    public class CommandServices : ICommandServices
    {

        public CommandServices(ICommandRepository repository, IMapper mapper)
        {
            this.Repository = repository;
            Mapper = mapper;
        }

        public ICommandRepository Repository { get; }
        public IMapper Mapper { get; }

        public async Task<CommandEntity> CreateCommandAsync(CommandCreate command)
        {

            var commandEntity = Mapper.Map<CommandEntity>(command);
            await this.Repository.CreateCommandAsync(commandEntity);
            await SaveChangesAsync();

            return commandEntity;
        }

        public async Task<IEnumerable<CommandEntity>> GetAllCommandsAsync()
        {
                
                var commands = await this.Repository.GetAllCommandsAsync();

                return commands;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.Repository.SaveChangesAsync();
        }

        public async Task<CommandEntity> GetCommandById(int id)
        {
            if (id < 0)
            {
                return null;
            }

            var commandEntity = await this.Repository.GetCommandById(id);

            if (commandEntity != null)
            {
                return commandEntity;
            }
            return null;
        }

        public async Task UpdateCommand(CommandUpdate command, int id)
        {
            var originalCommand = await GetCommandById(id);

            if (originalCommand == null)
            {
                return;
            }

            var updatedCommmand = Mapper.Map(command, originalCommand);

            await Repository.UpdateCommand(updatedCommmand);
            await Repository.SaveChangesAsync();
        }

        public async Task DeleteCommandById(CommandEntity command)
        {
            await Repository.DeleteCommandById(command);
        }
    }
}