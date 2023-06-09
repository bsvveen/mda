
using MDA.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MDA.Admin
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminSql _adminSql;

        public AdminController(IAdminSql adminSql)
        {
            _adminSql = adminSql;
        }


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
            AdminServices adminservice = new(_adminSql);

            var syncSuccess = adminservice.SyncWithDatabase();

            return Ok(syncSuccess);
        }
    }
}