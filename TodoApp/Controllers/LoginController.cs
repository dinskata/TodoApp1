using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using Microsoft.AspNetCore.Http;

public class LoginController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        
        if (user == null)
        {
            ViewBag.Error = "Invalid username or password";
            return View("Index");
        }

        _httpContextAccessor.HttpContext!.Session.SetInt32("UserId", user.Id);
        
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        _httpContextAccessor.HttpContext!.Session.Remove("UserId");
        return RedirectToAction("Index", "Login");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}