
using Microsoft.AspNetCore.Mvc;

namespace MDA.Admin
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {  
        [HttpGet("GetModel")]
        public IActionResult GetModel()
        {
            AdminServices adminservice = new();

            var model = adminservice.Model;

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPost("UpdateModel")]
        public IActionResult UpdateModel(Primitive newModel)
        {
            AdminServices adminservice = new();

            var updatedPrimitive = adminservice.UpdateModel(newModel);

            return Ok(updatedPrimitive);
        }
    }
}