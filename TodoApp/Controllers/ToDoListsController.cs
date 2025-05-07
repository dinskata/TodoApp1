using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;

public class ToDoListsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ToDoListsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: ToDoLists
    public IActionResult Index()
    {
        var userId = GetCurrentUserId();
        var lists = _context.ToDoLists
            .Where(l => l.CreatedById == userId || 
                       _context.ListShares.Any(ls => ls.ToDoListId == l.Id && ls.UserId == userId))
            .Include(l => l.CreatedBy)
            .Include(l => l.Tasks)
            .ToList();

        return View(lists);
    }

    // GET: ToDoLists/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null || !CanAccessList(id.Value))
        {
            return NotFound();
        }

        var toDoList = _context.ToDoLists
            .Include(l => l.CreatedBy)
            .Include(l => l.LastModifiedBy)
            .FirstOrDefault(m => m.Id == id);

        if (toDoList == null)
        {
            return NotFound();
        }

        return View(toDoList);
    }

    // GET: ToDoLists/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ToDoLists/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title")] ToDoList toDoList)
    {
        if (ModelState.IsValid)
        {
            var userId = GetCurrentUserId();
            toDoList.CreatedAt = DateTime.Now;
            toDoList.CreatedById = userId;
            toDoList.LastModifiedAt = DateTime.Now;
            toDoList.LastModifiedById = userId;

            _context.Add(toDoList);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(toDoList);
    }

    // GET: ToDoLists/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null || !CanAccessList(id.Value))
        {
            return NotFound();
        }

        var toDoList = _context.ToDoLists.Find(id);
        if (toDoList == null)
        {
            return NotFound();
        }
        return View(toDoList);
    }

    // POST: ToDoLists/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Title")] ToDoList toDoList)
    {
        if (id != toDoList.Id || !CanAccessList(id))
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingList = _context.ToDoLists.Find(id);
                existingList!.Title = toDoList.Title;
                existingList.LastModifiedAt = DateTime.Now;
                existingList.LastModifiedById = GetCurrentUserId();

                _context.Update(existingList);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoListExists(toDoList.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(toDoList);
    }

    // GET: ToDoLists/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null || !CanAccessList(id.Value))
        {
            return NotFound();
        }

        var toDoList = _context.ToDoLists
            .Include(l => l.CreatedBy)
            .FirstOrDefault(m => m.Id == id);

        if (toDoList == null)
        {
            return NotFound();
        }

        return View(toDoList);
    }

    // POST: ToDoLists/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!CanAccessList(id))
        {
            return NotFound();
        }

        var toDoList = _context.ToDoLists.Find(id);
        if (toDoList == null)
        {
            return NotFound();
        }

        if (toDoList.CreatedById == GetCurrentUserId())
        {
            // Delete the list if owner
            _context.ToDoLists.Remove(toDoList);
        }
        else
        {
            // Remove share if not owner
            var share = _context.ListShares.FirstOrDefault(ls => ls.ToDoListId == id && ls.UserId == GetCurrentUserId());
            if (share != null)
            {
                _context.ListShares.Remove(share);
            }
        }

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    // GET: ToDoLists/Share/5
    public IActionResult Share(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var toDoList = _context.ToDoLists.Find(id);
        if (toDoList == null || toDoList.CreatedById != GetCurrentUserId())
        {
            return NotFound();
        }

        var sharedWith = _context.ListShares
            .Where(ls => ls.ToDoListId == id)
            .Include(ls => ls.User)
            .ToList();

        var availableUsers = _context.Users
            .Where(u => u.Id != GetCurrentUserId() && 
                       !sharedWith.Any(sw => sw.UserId == u.Id))
            .ToList();

        ViewBag.ListId = id;
        ViewBag.ListTitle = toDoList.Title;
        ViewBag.SharedWith = sharedWith;
        ViewBag.AvailableUsers = availableUsers;

        return View();
    }

    // POST: ToDoLists/Share/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Share(int listId, int userId)
    {
        if (!_context.ToDoLists.Any(l => l.Id == listId && l.CreatedById == GetCurrentUserId()))
        {
            return NotFound();
        }

        if (!_context.ListShares.Any(ls => ls.ToDoListId == listId && ls.UserId == userId))
        {
            _context.ListShares.Add(new ListShare 
            { 
                ToDoListId = listId, 
                UserId = userId,
                SharedAt = DateTime.Now
            });
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Share), new { id = listId });
    }

    // POST: ToDoLists/Unshare/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Unshare(int listId, int userId)
    {
        var share = _context.ListShares
            .FirstOrDefault(ls => ls.ToDoListId == listId && ls.UserId == userId);

        if (share != null)
        {
            _context.ListShares.Remove(share);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Share), new { id = listId });
    }

    private bool ToDoListExists(int id)
    {
        return _context.ToDoLists.Any(e => e.Id == id);
    }

    private bool CanAccessList(int listId)
    {
        var userId = GetCurrentUserId();
        return _context.ToDoLists.Any(l => l.Id == listId && 
                                         (l.CreatedById == userId || 
                                          _context.ListShares.Any(ls => ls.ToDoListId == listId && ls.UserId == userId)));
    }

    private int GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext!.Session.GetInt32("UserId") ?? 0;
    }
}