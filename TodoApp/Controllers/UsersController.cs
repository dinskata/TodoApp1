using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Added this
using System;
using System.Linq;
using TodoApp.Models;
using Microsoft.AspNetCore.Http;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsersController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private bool IsAdmin()
    {
        var userId = _httpContextAccessor.HttpContext!.Session.GetInt32("UserId");
        return userId == 1; 
    }

    private int GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext!.Session.GetInt32("UserId") ?? 0;
    }

   public IActionResult Index()
{
    if (!IsAdmin()) 
    {
        return RedirectToAction("Index", "Home");
    }
    return View(_context.Users.ToList());
}

    public IActionResult Create()
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Login");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(User user)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Login");

        if (ModelState.IsValid)
        {
            user.CreatedAt = DateTime.Now;
            user.CreatedBy = GetCurrentUserId();
            user.LastModified = DateTime.Now;
            user.LastModifiedBy = GetCurrentUserId();
            
            _context.Add(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    public IActionResult Edit(int? id)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Login");
        if (id == null) return NotFound();
        
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();
        
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, User user)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Login");
        if (id != user.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                user.LastModified = DateTime.Now;
                user.LastModifiedBy = GetCurrentUserId();
                
                _context.Update(user);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    public IActionResult Delete(int? id)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Login");
        if (id == null) return NotFound();
        
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();
        
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Login");
        
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();
        
        _context.Users.Remove(user);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}