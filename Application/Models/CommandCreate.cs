using System.ComponentModel.DataAnnotations;

namespace Commands.Application.Models
{
    public class CommandCreate
    {
        [Required]        
        public string Command { get; set; }

        [Required]
        public string Description { get; set; }
    }
}