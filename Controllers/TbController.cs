using Microsoft.AspNetCore.Mvc;
using XlcToolBox.Services;

namespace XlcToolBox.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TbController : ControllerBase
    {

        private TbService _tbService = new TbService();

        [HttpGet, Route("trans")]
        public string trans(string token)
        {
            return _tbService.Trans(token);
        }

        [HttpGet, Route("prasetkl")]
        public string prasetkl(string token)
        {
            return _tbService.prasetkl(token);
        }
    }
}
