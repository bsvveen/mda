﻿
using MDA.Admin;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MDA.Infrastructure
{
    public class SubmitRequest
    {
        [Required]
        public string? Entity { get; set; }

        public Guid? Id { get; set; }

        public Dictionary<string,string>? Properties { get; set; }

        public bool IsValid
        {
            get
            {
                var model = new AdminServices().Model;
                var entity = model.Entities.SingleOrDefault(tbl => tbl.Name == Entity);

                if (entity != null)
                {
                    var AllPropertiesExists = (Properties != null) && Properties.All(reqProp => entity.Properties.Any(entProp => entProp.Name.Equals(reqProp.Key)));
                    return AllPropertiesExists;
                }

                return false;
            }
        }       
    }  
}
