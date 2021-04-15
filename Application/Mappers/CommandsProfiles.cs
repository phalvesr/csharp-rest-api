using AutoMapper;
using Commands.Application.Models;
using Commands.Domain.Entities;

namespace Commands.Application.Mappers
{
    public class CommandsProfiles : Profile
    {
        public CommandsProfiles()
        {
            CreateMap<CommandEntity, CommandResponse>();
            CreateMap<CommandCreate, CommandEntity>();
            CreateMap<CommandUpdate, CommandEntity>();
            CreateMap<CommandEntity, CommandUpdate>();
        }        
    }
}