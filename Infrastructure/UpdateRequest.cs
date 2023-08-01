
using MDA.Admin;
using System.ComponentModel.DataAnnotations;

namespace MDA.Infrastructure
{
    public class UpdateRequest
    {
        public UpdateRequest()
        {
            Properties = new Dictionary<string, object>();
        }

        [Required]
        public string? EntityName { get; set; }

        [Required]
        public Guid Id { get; set; }

        public Dictionary<string,object>? Properties { get; set; }       

        public void Validate()
        {
        }
    }  
}
