using FluentValidation;
using MDA.Infrastructure;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MDA.Dbo
{ 
    public class AddColumn
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.TableName).NotEmpty().WithMessage("TableName is required");
                RuleFor(m => m.ColumnName).NotEmpty().WithMessage("ColumnName is required");
                RuleFor(m => m.ColumnDataType).NotEmpty().WithMessage("ColumnDataType is required");
            }
        }

        public class Query : IRequest<ActionResponse>
        {
            [Required]
            public string? TableName { get; set; }

            [Required]
            public string? ColumnName { get; set; }

            [Required]
            public ColumnDataType? ColumnDataType { get; set; }

            public ColumnConstrains? ColumnConstrains { get; set; }            
        }

        public class QueryHandler : IRequestHandler<Query, ActionResponse>
        {            
            public Task<ActionResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var sqlQuery = $"ALTER TABLE {request.TableName} ADD {request.ColumnName} {request.ColumnDataType?.Value} {request.ColumnConstrains?.Value}";
                return Sqlite.ExecuteSql(sqlQuery);
            }
        }
    }
}
