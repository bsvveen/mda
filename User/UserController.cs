
using MDA.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MDA.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {       

        [HttpGet("GetModelSchema")]
        public IActionResult GetModelSchema()
        {
            var modelSchema = new UserServices().ModelJSchema;

            if (modelSchema == null)
                return NotFound();
            
            return Ok(modelSchema.ToString());
        }

        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] ListRequest request)
        {
            var userService = new UserServices();

            var requestIsValid = userService.validateRequest(request);
            if (!requestIsValid)
            {
                return BadRequest("GetRequest is not valid, probably an entityname or propertyname does not exists in the model");
            }

            return Ok(userService.List(request).Result);
        }       
    }
}