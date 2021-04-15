using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commands.Domain.Entities
{
    public class CommandEntity
    {
        
        [Key]
        public int Id { get; set; }

        [MaxLength(300)]
        [Required]
        public string Command { get; set; }

        [MaxLength(300)]
        [Required]
        public string Description { get; set; }
    }
}