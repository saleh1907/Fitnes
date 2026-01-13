using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Areas.admin.Controllers;
[Area("Admin")]

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
