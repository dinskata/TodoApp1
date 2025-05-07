using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;

public class TasksController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TasksController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: Tasks/Index?listId=2
    public IActionResult Index(int listId)
    {
        if (!CanAccessList(listId))
            return NotFound();

        var tasks = _context.TaskItems
            .Where(t => t.ToDoListId == listId)
            .Include(t => t.CreatedBy)
            .Include(t => t.LastModifiedBy)
            .ToList();

        ViewBag.ListId = listId;
        ViewBag.ListTitle = _context.ToDoLists.Find(listId)?.Title;
        return View(tasks);
    }

    // GET: Tasks/Create?listId=2
    public IActionResult Create(int listId)
    {
        if (!CanAccessList(listId))
            return NotFound();

        ViewBag.ListId = listId;
        return View();
    }

    // POST: Tasks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title,Description,ToDoListId")] TaskItem taskItem)
    {
        if (ModelState.IsValid)
        {
            var userId = GetCurrentUserId();
            taskItem.CreatedAt = DateTime.Now;
            taskItem.CreatedById = userId;
            taskItem.LastModifiedAt = DateTime.Now;
            taskItem.LastModifiedById = userId;
            taskItem.IsComplete = false;

            _context.Add(taskItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { listId = taskItem.ToDoListId });
        }
        ViewBag.ListId = taskItem.ToDoListId;
        return View(taskItem);
    }

    // GET: Tasks/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var taskItem = _context.TaskItems.Find(id);
        if (taskItem == null || !CanAccessList(taskItem.ToDoListId))
            return NotFound();

        return View(taskItem);
    }

    // POST: Tasks/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Title,Description,IsComplete,ToDoListId")] TaskItem taskItem)
    {
        if (id != taskItem.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var existingTask = _context.TaskItems.Find(id);
                existingTask!.Title = taskItem.Title;
                existingTask.Description = taskItem.Description;
                existingTask.IsComplete = taskItem.IsComplete;
                existingTask.LastModifiedAt = DateTime.Now;
                existingTask.LastModifiedById = GetCurrentUserId();

                _context.Update(existingTask);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(taskItem.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index), new { listId = taskItem.ToDoListId });
        }
        return View(taskItem);
    }

    // GET: Tasks/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var taskItem = _context.TaskItems
            .Include(t => t.ToDoList)
            .FirstOrDefault(t => t.Id == id);
        
        if (taskItem == null || !CanAccessList(taskItem.ToDoListId))
            return NotFound();

        return View(taskItem);
    }

    // POST: Tasks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var taskItem = _context.TaskItems.Find(id);
        if (taskItem == null || !CanAccessList(taskItem.ToDoListId))
            return NotFound();

        _context.TaskItems.Remove(taskItem);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index), new { listId = taskItem.ToDoListId });
    }

    // POST: Tasks/ToggleComplete/5
    [HttpPost]
    public IActionResult ToggleComplete(int id)
    {
        var taskItem = _context.TaskItems.Find(id);
        if (taskItem == null || !CanAccessList(taskItem.ToDoListId))
            return NotFound();

        taskItem.IsComplete = !taskItem.IsComplete;
        taskItem.LastModifiedAt = DateTime.Now;
        taskItem.LastModifiedById = GetCurrentUserId();
        
        _context.Update(taskItem);
        _context.SaveChanges();
        
        return RedirectToAction(nameof(Index), new { listId = taskItem.ToDoListId });
    }

    private bool TaskItemExists(int id)
    {
        return _context.TaskItems.Any(e => e.Id == id);
    }

    private bool CanAccessList(int ToDoListId)
    {
        var userId = GetCurrentUserId();
        return _context.ToDoLists.Any(l => l.Id == ToDoListId && 
                                         (l.CreatedById == userId || 
                                          _context.ListShares.Any(ls => ls.ToDoListId == ToDoListId && ls.UserId == userId)));
    }

    private int GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext!.Session.GetInt32("UserId") ?? 0;
    }

    
}