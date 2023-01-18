
using MDA.Admin;
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
            var model = new AdminServices().Model;

            if (model == null)
                return NotFound();
            
            return Ok(model);
        }

        [HttpPost("List")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            if (!request.IsValid)
            {
                return BadRequest(request.Errors);
            }

            var userService = new UserServices(request.Entity);
            var stringResponse = await userService.List(request);
            Response.StatusCode = 200;
            return Content(stringResponse, "application/json");
        }

        [HttpPost("GetById")]
        public async Task<IActionResult> GetById([FromBody] GetByIdRequest request)
        {    
            if (!request.IsValid)
            {
                return BadRequest("GetByIdRequest is not valid, probably an entityname or ID does not exists in the model");
            }

            var userService = new UserServices(request.Entity);
            var stringResponse = await userService.GetById(request);
            Response.StatusCode = 200;
            return Content(stringResponse, "application/json");
        }

        [HttpPost("Submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitRequest request)
        {
            if (!request.IsValid)
            {
                return BadRequest(request.Errors);
            }

            var userService = new UserServices(request.Entity);
            var intResponse = await userService.Submit(request);
            Response.StatusCode = 200;
            return Content(intResponse.ToString(), "application/json");
        }
    }
}