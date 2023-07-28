
using MDA.Infrastructure;
using Microsoft.AspNetCore.Mvc;

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
            if (!ModelState.IsValid)
                return Conflict(ModelState);

            if (!request.IsValid())
                return BadRequest("Fout in ListRequest: " + string.Join(",", request.Errors));

            var requestAccessValidation = _model.CheckAuthorization(request.EntityName, request.Properties);
            if (!requestAccessValidation.IsValid)
                return Forbid("Access Denied");      

            var userService = new UserServices(_model);            
            return Ok(await userService.List(request));           
        }

        [HttpPost("GetById")]
        public async Task<IActionResult> GetById([FromBody] GetByIdRequest request)
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState);

            if (!request.IsValid())
                return BadRequest("Fout in GetByIdRequest: " + string.Join(",", request.Errors));

            var requestAccessValidation = _model.CheckAuthorization(request.EntityName, request.Properties);
            if (!requestAccessValidation.IsValid)
                return Forbid("Access Denied");                   

            var userService = new UserServices(_model);
            return Ok(await userService.GetById(request));                  
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateRequest request)
        { 
            var validationResult = request.Validate();

            if (validationResult.Errors.Count > 0)
                return BadRequest(validationResult.Errors);

            if (validationResult.ValidationErrors.Count > 0)
                return Conflict(validationResult.ValidationErrors);

            var requestAccessValidation = _model.CheckAuthorization(request.EntityName, request.Properties.Select(p => p.Key).ToList());
            if (!requestAccessValidation.IsValid)
                return Forbid("Access Denied");

            var requestValueValidation = _model.CheckValuesValidity(request.EntityName, request.Properties);
            if (!requestValueValidation.IsValid)
                return Conflict(requestValueValidation.ValidationErrors);

            var userService = new UserServices(_model);
            return Ok(await userService.Create(request));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateRequest request)
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState);
            
            if (!request.IsValid())
                return BadRequest("Fout in UpdateRequest: " + string.Join(",", request.Errors));

            var requestAccessValidation = _model.CheckAuthorization(request.EntityName, request.Properties.Select(p => p.Key).ToList());
            if (!requestAccessValidation.IsValid)
                return Forbid("Access Denied");

            var requestValueValidation = _model.CheckValuesValidity(request.EntityName, request.Properties);
            if (!requestValueValidation.IsValid)
                return Conflict(requestValueValidation.ValidationErrors);            

            var userService = new UserServices(_model);
            await userService.Update(request);
            Response.StatusCode = 200;

            return Ok("Update Succeeded");
        }
    }
}