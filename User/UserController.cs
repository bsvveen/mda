
using MDA.Admin;
using MDA.Infrastructure;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserSql _userSql;       

        public UserController(ApplicationInstance application, IUserSql userSql)
        {
            _model = application.Model;
            _userSql = userSql;
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

            var userService = new UserServices(_model, _userSql);            
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

            var userService = new UserServices(_model, _userSql);
            return Ok(await userService.GetById(request));                  
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] SubmitRequest request)
        {
            if (!ModelState.IsValid)
                return Conflict(ModelState);
            
            if (!request.IsValid())
                return BadRequest("Fout in SubmitRequest: " + string.Join(",", request.Errors));

            var requestAccessValidation = _model.CheckAuthorization(request.EntityName, request.Properties.Select(p => p.Key).ToList());
            if (!requestAccessValidation.IsValid)
                return Forbid("Access Denied");

            var requestValueValidation = _model.CheckValuesValidity(request.EntityName, request.Properties);
            if (!requestValueValidation.IsValid)
                return Conflict(requestValueValidation.ValidationErrors);            

            var userService = new UserServices(_model, _userSql);
            await userService.Submit(request);
            Response.StatusCode = 200;
            return Ok("Update Succeeded");
        }
    }
}