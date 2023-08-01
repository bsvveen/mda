
using MDA.Admin;
using System.ComponentModel.DataAnnotations;

namespace MDA.Infrastructure
{
    public class ListRequest
    {
        public ListRequest()
        {
            Properties = new List<string>();
        }

        [Required]
        public string? EntityName { get; set; }
       
        public List<string> Properties { get; set; }

        public List<Constrain>? Constrains { get; set; }       

        public void Validate()
        {
        }
    }  
}
