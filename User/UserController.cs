
using MDA.Admin;
using MDA.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using static MDA.Infrastructure.Primitive;

namespace MDA.User
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly Primitive _model;

        public UserController(ApplicationInstance application)
        {
            _model = application.Model;
        }

        [HttpGet("GetModel")]
        public IActionResult GetModel()
        {  
            return Ok(_model);
        }

        [HttpPost("List")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var requestFormatValidation = _model.CheckExistence(request.Entity, request.Properties);
            if (!requestFormatValidation.IsValid)            
                return BadRequest("Fout in ListRequest: " + string.Join(",", requestFormatValidation.Errors));

            var requestValueValidation = _model.CheckValuesValidity(request.Entity, request.Properties);
            if (!requestValueValidation.IsValid)
                return Conflict(requestFormatValidation.ValidationErrors);

            var requestAccessValidation = _model.CheckAuthorization(request.Entity, request.Properties);
            if (!requestAccessValidation.IsValid)
                return Forbid("Access Denied");

            var userService = new UserServices(_model);            
            return Ok(await userService.List(request));           
        }

        [HttpPost("GetById")]
        public async Task<IActionResult> GetById([FromBody] GetByIdRequest request)
        {    
            if (!request.IsValid)            
                return BadRequest("GetByIdRequest is not valid, probably an entityname or ID does not exists in the model");
           
            var userService = new UserServices(_model);
            return (IActionResult)Results.Json(await userService.GetById(request));           
        }

        [HttpPost("Submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitRequest request)
        {  
            if (!request.IsValid())
            {                
                return BadRequest("Fout in SubmitRequest: " + string.Join(",", request.Errors));
            }

            var userService = new UserServices(_model);
            var intResponse = await userService.Submit(request);
            Response.StatusCode = 200;
            return Content(intResponse.ToString(), "application/json");
        }
    }
}