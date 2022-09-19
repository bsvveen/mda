
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
            AdminServices adminservice = new AdminServices();

            if (adminservice.Model == null)
                return NotFound();

            return Ok(adminservice.Model);
        }

        [HttpPost("UpdateModel")]
        public IActionResult UpdateModel(Primitive newModel)
        {
            AdminServices adminservice = new AdminServices();

            var updatedPrimitive = adminservice.UpdateModel(newModel);

            return Ok(updatedPrimitive);
        }
    }
}