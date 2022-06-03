
using MDA.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MDA.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {       

        [HttpGet("GetPrimitiveSchema")]
        public IActionResult GetModelSchema()
        {
            var modelSchema = new DataHandler().AsJSchema;

            if (modelSchema == null)
                return NotFound();
            
            return Ok(modelSchema.ToString());
        }

        [HttpPost("GetModel")]
        public async Task<IActionResult> GetModel([FromBody] GetRequest request)
        {
            var modelHandler = new DataHandler();

            var requestIsValid = modelHandler.validateRequest(request);
            if (!requestIsValid)
            {
                return BadRequest("GetRequest is not valid, probably an entityname or propertyname does not exists in the model");
            }

            return Ok(modelHandler.List(request).Result);
        }       
    }
}