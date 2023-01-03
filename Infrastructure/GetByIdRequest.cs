
using MDA.Admin;
using System.ComponentModel.DataAnnotations;

namespace MDA.Infrastructure
{
    public class GetByIdRequest
    {
        [Required]
        public string? Entity { get; set; }

        [Required]
        public Guid? Id { get; set; }

        public List<string>? Properties { get; set; }

        public bool IsValid
        {
            get
            {
                var model = new AdminServices().Model;
                var entity = model.Entities.SingleOrDefault(tbl => tbl.Name == Entity);

                if (entity != null)
                    return true;

                return false;
            }            
        }
    }    
}