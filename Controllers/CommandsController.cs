using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Commands.Application.Models;
using Commands.Domain.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commands.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CommandsController : ControllerBase
    {

        public ICommandServices CommandsServices { get; }
        public IMapper Mapper { get; }

        public CommandsController(ICommandServices commandsServices, IMapper mapper)
        {
            this.CommandsServices = commandsServices;
            this.Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandResponse>>> GetAllCommands()
        {

            var commandEntities = await CommandsServices.GetAllCommandsAsync();

            var response = Mapper.Map<IEnumerable<CommandResponse>>(commandEntities);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CommandResponse>> CreateCommand(CommandCreate command)
        {
            var commandResponse = await this.CommandsServices.CreateCommandAsync(command);

            Console.WriteLine(commandResponse.Id);

            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandResponse.Id }, commandResponse);
        }

        [HttpGet("get-by-id", Name = "GetCommandById")]
        public async Task<ActionResult<CommandResponse>> GetCommandById([FromQuery] int id)
        {
            var response = await this.CommandsServices.GetCommandById(id);

            if (response == null)
            {
                return BadRequest
                (
                    new
                    {
                        StatusCode = 400,
                        ServerErrorMessage = $"We could not find a register with the id {id}. Check your id again."
                    }
                );
            }

            var mappedResponse = Mapper.Map<CommandResponse>(response);

            return Ok(mappedResponse);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCommand(CommandUpdate command, int id)
        {
            await CommandsServices.UpdateCommand(command, id);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommandById(int id)
        {
            var command = await CommandsServices.GetCommandById(id);

            if (command == null)
            {
                return BadRequest
                (
                    new 
                    {
                        mensagem = $"Não foi possível encontrar o comando com o id {id}",
                        statusCode = HttpStatusCode.BadRequest
                    }
                );
            }

            await CommandsServices.DeleteCommandById(command);
            
            return NoContent();        
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCommand(int id, [FromBody] JsonPatchDocument<CommandUpdate> patchDocument)
        {
            // To build a patch endpoint in C# we need JSONPatch package for ASPNET Core
            // and NewtonsoftJson 

            // Check if we have that command
            var registerComand = await CommandsServices.GetCommandById(id);

            if (registerComand is null)
            {
                return NotFound();
            }

            // Map our command to a command update
            var commandToPatch = Mapper.Map<CommandUpdate>(registerComand);
            
            // Apply out JsonPatch to out command! 
            patchDocument.ApplyTo(commandToPatch, ModelState);

            // Check if everything went well
            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            // Update our command!
            await CommandsServices.UpdateCommand(commandToPatch, id);
            await CommandsServices.SaveChangesAsync();

            // Return NoContent as specified by REST especifications
            return NoContent();
        }
    }
}