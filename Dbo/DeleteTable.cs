using FluentValidation;
using MDA.Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MDA.Dbo
{ 
    public class DeleteTable
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.TableName).NotEmpty().WithMessage("TableName is required");
            }
        }

        public class Query : IRequest<ActionResponse>
        {
            [Required]
            public string? TableName { get; set; }           
        }

        public class QueryHandler : IRequestHandler<Query, ActionResponse>
        {            
            public Task<ActionResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                return Sqlite.ExecuteSql($"DROP TABLE {request.TableName};");
            }
        }
    }
}
