
using Microsoft.AspNetCore.Mvc;

namespace MDA.Admin
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    { 
        [HttpGet("GetPrimitiveSchema")]
        public IActionResult GetPrimitiveSchema()
        {
            var primitiveSchema = Primitive.AsJSchema;
            
            if (primitiveSchema == null)
                return NotFound();
            
            return Ok(primitiveSchema.ToString());
        }

        [HttpGet("GetModel")]
        public IActionResult GetModel()
        {
            AdminServices mh = new AdminServices();

            if (mh.Model == null)
                return NotFound();

            return Ok(mh.Model);
        }

        [HttpPost("UpdateModel")]
        public IActionResult UpdateModel(Primitive newModel)
        {
            AdminServices mh = new AdminServices();

            var updatedPrimitive = mh.UpdateModel(newModel);

            return Ok(updatedPrimitive);
        }
    }
}