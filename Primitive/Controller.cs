
using MDA.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MDA.Dbo
{
    [ApiController]
    [Route("[controller]")]
    public class Controller : ControllerBase
    {
        public ISql Sql { get; }

        public Controller(ISql sql)
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
        public async Task<IActionResult> GetModel()
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