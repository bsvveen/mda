
using MDA.Primitive.Database;
using Microsoft.AspNetCore.Mvc;

namespace MDA.Model
{
    [ApiController]
    [Route("[controller]")]
    public class ModelController : ControllerBase
    {
        public ISql Sql { get; }

        public ModelController(ISql sql)
        {
            Sql = sql;
        }

        [HttpGet("GetPrimitiveSchema")]
        public IActionResult GetModelSchema()
        {
            var modelSchema = new ModelHandler(Sql).AsJSchema;

            if (modelSchema == null)
                return NotFound();
            
            return Ok(modelSchema.ToString());
        }

        [HttpGet("GetModel")]
        public async Task<IActionResult> GetModel()
        {    
            throw new NotImplementedException();           
        }

        [HttpPost("UpdatePrimitive")]
        public async Task<IActionResult> UpdateModel(object newModel)
        {
            throw new NotImplementedException();
        }
    }
}