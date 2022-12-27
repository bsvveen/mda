
using MDA.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace MDA.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {  
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
                return BadRequest("ListRequest is not valid, probably an entityname or propertyname does not exists in the model");
            }

            var stringResponse = await userService.List(request);
            Response.StatusCode = 200;
            return Content(stringResponse, "application/json");
        }

        [HttpPost("Submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitRequest request)
        {
            var userService = new UserServices();

            var requestIsValid = userService.validateRequest(request);
            if (!requestIsValid)
            {
                return BadRequest("SubmitRequest is not valid, probably an entityname or propertyname does not exists in the model");
            }

            var intResponse = await userService.Submit(request);
            Response.StatusCode = 200;
            return Content(intResponse.ToString(), "application/json");
        }
    }
}