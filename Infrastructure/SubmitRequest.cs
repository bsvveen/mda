
using System.ComponentModel.DataAnnotations;

namespace MDA.Infrastructure
{
    public class SubmitRequest
    {
        [Required]
        public string? Entity { get; set; }

        public Guid? Id { get; set; }

        public Dictionary<string,string>? Properties { get; set; }       
    }  
}
