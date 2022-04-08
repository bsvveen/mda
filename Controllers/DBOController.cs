using MDA.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MDA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DBOController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public DBOController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost("AddTable")]
        public async Task<IActionResult> AddTable(Dbo.AddTable.Query request)
        {
            var result = await _mediatR.Send(request);
            if (result == null)
                return NotFound();
            if (!result.Succeeded)
                return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpPost("AddColumn")]
        public async Task<IActionResult> AddColumn(Dbo.AddColumn.Query request)
        {
            request.ColumnDataType = Dbo.ColumnDataType.CHAR_255;

            var result = await _mediatR.Send(request);
            if (result == null)
                return NotFound();
            if (!result.Succeeded)
                return BadRequest(result.Error);
            return Ok(result);
        }       
    }
}