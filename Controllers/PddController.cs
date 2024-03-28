using Microsoft.AspNetCore.Mvc;
using XlcToolBox.Services;

namespace XlcToolBox.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PddController : ControllerBase
    {

        private PddService _pddService = new PddService();

        [HttpGet, Route("trans")]
        public string trans(string url)
        {
            return _pddService.Trans(url);
        }
    }
}
