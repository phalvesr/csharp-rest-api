using System.ComponentModel.DataAnnotations;

namespace Commands.Application.Models
{
    public class CommandUpdate
    {

        [Required]
        public string Command { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}