
using MDA.Admin;
using System.ComponentModel.DataAnnotations;

namespace MDA.Infrastructure
{
    public class GetByIdRequest
    {     
        [Required]
        public string? EntityName { get; set; }

        [Required]
        public Guid? Id { get; set; }

        public bool IncludeRelations { get; set; } = false;

        public void Validate()
        {
        }
    }
}