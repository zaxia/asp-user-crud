using base_test_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace base_test_mvc.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Context _context;

    public HomeController(ILogger<HomeController> logger, Context context)
    {
        _logger = logger;
        _context = context;
    }

    // GET: Home/Index
    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_Login")))
        {
            return RedirectToAction("Login");
        }
        ViewData["Username"] = HttpContext.Session.GetString("_Login");
        return View();
    }

    // GET: Home/Login
    public IActionResult Login()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Login")))
        {
            return RedirectToAction("Index");
        }
        return PartialView();
    }

    // POST: Home/Login
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        User user = _context.User.Find(username);
        if(user == null)
        {
            return Json("user_not_found");
        }
        if (user.verifyPassword(password))
        {
            HttpContext.Session.SetString("_Login", user.login);
            return Json(user);
        }
        return Json("wrong_password");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
