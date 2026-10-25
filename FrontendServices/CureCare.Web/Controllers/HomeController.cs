using Microsoft.AspNetCore.Mvc;

namespace CureCare.Web.Controllers;

public class HomeController : Controller
{
    // CureCare dashboard open karta hai.
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}