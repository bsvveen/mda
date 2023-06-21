
using MDA.Infrastructure;
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
            var model = new ModelServices().Model;

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPost("UpdateModel")]
        public IActionResult UpdateModel(Primitive newModel)
        {  
            var updatedPrimitive = new ModelServices().UpdateModel(newModel);

            return Ok(updatedPrimitive);
        }

        [HttpPost("SyncDatabase")]
        public IActionResult SyncDatabase()
        {
            AdminServices adminservice = new();

            var syncSuccess = adminservice.SyncWithDatabase();

            return Ok("No error so sync succeeded");
        }
    }
}