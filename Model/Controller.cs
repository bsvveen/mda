
using MDA.Infrastructure;
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

        [HttpPost("GetModel")]
        public async Task<IActionResult> GetModel([FromBody] GetRequest request)
        {
            var modelHandler = new ModelHandler(Sql);

            return Ok(modelHandler.Get(request).Result);
        }       
    }
}