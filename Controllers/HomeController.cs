using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("")]
    public class HomeController : Controller {
        [HttpGet]
        public IActionResult Index () => View ();
    }
}