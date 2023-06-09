﻿
using MDA.Admin;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MDA.Infrastructure
{
    public class SubmitRequest
    {
        public SubmitRequest()
        {
            Properties = new Dictionary<string, object>();
            Errors = new List<string>();
        }

        [Required]
        public string? EntityName { get; set; }

        public Guid? Id { get; set; }

        public Dictionary<string,object>? Properties { get; set; }

        public List<string>? Errors { get; set; }

        public bool IsValid()
        {            
            var model = new ModelServices().Model;
            var entityModel = model.Entities.SingleOrDefault(tbl => tbl.Name == EntityName);

            if (entityModel == null)
            {
                Errors.Add($"Entity '{EntityName}' does not exists in de model");
                return false;
            }

            var notInModel = Properties.Select(p => p.Key).Except(entityModel.Properties.Select(p => p.Key));
            if (notInModel.Any())
                notInModel.ToList().ForEach(i => Errors.Add($"Property '{i}' does not exists on Entity '{EntityName}' "));           
            
            if (Properties.Any(p => p.Key == "Id"))
                Errors.Add($"The submit request properties collection should not contain an Id");

            return !Errors.Any();

        }       
    }  
}
