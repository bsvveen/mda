
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


        [HttpGet("GetModel")]
        public IActionResult GetModel()
        {
            var model = new UserServices().Model;

            if (model == null)
                return NotFound();
            
            return Ok(model);
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

        [HttpPost("Submit")]
        public async Task<IActionResult> Submit([FromBody] dynamic request)
        { 
            return Ok(request);
        }
    }
}