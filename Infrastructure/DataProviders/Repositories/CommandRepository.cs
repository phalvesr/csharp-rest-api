using System.Collections.Generic;
using System.Threading.Tasks;
using Commands.Domain.Entities;
using Commands.Domain.Repositories;
using Commands.Infrastructure.DataProviders.Databases;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using Commands.Application.Models;

namespace Commands.Infrastructure.DataProviders.Repositories
{
    public class CommandRepository : ICommandRepository
    {

        public CommandContext Context { get; }
        public CommandRepository(CommandContext context)
        {
            this.Context = context;
        }

        public async Task<IEnumerable<CommandEntity>> GetAllCommandsAsync()
        {
            try
            {
                var query = "SELECT * FROM commands;";

                var result = await this.Context
                                    .Database
                                    .GetDbConnection()
                                    .QueryAsync<CommandEntity>(query);

                return result;
            }
            catch
            {
                return new List<CommandEntity> { };
            }
        }

        public async Task CreateCommandAsync(CommandEntity command)
        {
            try
            {

                var connection = Context
                                .Database
                                .GetDbConnection();

                var queryToCreate =
                @"
                    INSERT INTO commands
                    (Command, Description)
                    VALUES 
                    (@Command, @Description);
                ";

                await connection.ExecuteAsync(queryToCreate,
                new
                {
                    Command = command.Command,
                    Description = command.Description
                });
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                System.Console.WriteLine("Erro ao criar dados");
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            var changedRows = await this.Context.SaveChangesAsync();

            return (changedRows > 0);
        }

        public async Task<CommandEntity> GetCommandById(int id)
        {
            try
            {
                var connection = this.Context
                                 .Database
                                 .GetDbConnection();

                var querySelectById =
                @"
                    SELECT * FROM commands
                    WHERE Id = @IdSearch;
                ";

                var entity = await connection
                             .QueryFirstOrDefaultAsync<CommandEntity>(querySelectById, new { IdSearch = id });

                if (entity == null)
                {
                    throw new Exception();
                }

                return entity;
            }
            catch
            {
                Console.WriteLine($"Erro ao buscar por ID {id}");
                return null;
            }
        }

        public async Task UpdateCommand(CommandEntity command)
        {
            try
            {
                var query = @"UPDATE commands
                              SET command = @CommandUpdate, Description = @DescriptionUpdate
                              WHERE Id = @IdUpdate;
                            ";
                var connection = Context
                                .Database
                                .GetDbConnection();

                await connection.ExecuteAsync
                (query,
                    new
                    {
                        CommandUpdate = command.Command,
                        DescriptionUpdate = command.Description,
                        IdUpdate = command.Id
                    }
                );
            }
            catch
            {
                return;
            }
        }

        public async Task DeleteCommandById(CommandEntity command)
        {
            try
            {
                var id = command.Id;
                var query = 
                @"
                    DELETE FROM commands
                    WHERE Id = @IdParametro;
                ";

                var connection = Context.Database.GetDbConnection();

                await connection.ExecuteAsync(query, new { @IdParametro = id });
            }
            catch
            {
                return;
            }
        }
    }
}
