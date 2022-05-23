
using MDA.Primitive.Database;
using Microsoft.AspNetCore.Mvc;

namespace MDA.Primitive
{
    [ApiController]
    [Route("[controller]")]
    public class PrimitiveController : ControllerBase
    {
        public ISql Sql { get; }

        public PrimitiveController(ISql sql)
        {
            Sql = sql;
        }

        [HttpGet("GetPrimitiveSchema")]
        public IActionResult GetPrimitiveSchema()
        {
            var modelSchema = new PrimitiveHandler(Sql).AsJSchema;

            if (modelSchema == null)
                return NotFound();
            
            return Ok(modelSchema.ToString());
        }

        [HttpGet("GetPrimitive")]
        public async Task<IActionResult> GetPrimitive()
        {
            PrimitiveHandler mh = new PrimitiveHandler(Sql);  

            if (mh.Primitive == null)
                return NotFound();           
            
            return Ok(mh.Primitive);
        }

        [HttpPost("UpdatePrimitive")]
        public async Task<IActionResult> UpdatePrimitive(Primitive newPrimitive)
        {
            PrimitiveHandler mh = new PrimitiveHandler(Sql);

            var updatedPrimitive = mh.UpdatePrimitive(newPrimitive);

            return Ok(updatedPrimitive);
        }
    }
}