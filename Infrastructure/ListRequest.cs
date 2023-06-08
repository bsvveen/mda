
using MDA.Admin;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MDA.Infrastructure
{
    public class ListRequest
    {
        public ListRequest() => Properties = new List<string>();

        [Required]
        public string? Entity { get; set; }
       
        public List<string> Properties { get; set; }

        public List<Constrain>? Constrains { get; set; }      
    }  
}
