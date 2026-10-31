using Microsoft.AspNetCore.Mvc;

namespace SmartPayPark.Api.Controllers
{
    public class AdminOperatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
