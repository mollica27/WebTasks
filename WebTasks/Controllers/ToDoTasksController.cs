using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WebTasks.Data;
using WebTasks.Models;

namespace WebTasks.Controllers
{
    public class ToDoTasksController : Controller
    {
        private readonly WebTasksContext _context;

        public ToDoTasksController(WebTasksContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ToDoTask.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Task not found." });
            }

            var toDoTask = await _context.ToDoTask.FirstOrDefaultAsync(m => m.Id == id);
            if (toDoTask == null)
            {
                return Json(new { success = false, message = "Task not found." });
            }

            return View(toDoTask);
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            var task = _context.ToDoTask.FirstOrDefault(t => t.Title.Contains(query));
            if (task == null)
            {
                TempData["AlertMessage"] = "Task not found.";
                TempData["AlertSuccess"] = false;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Details", new { id = task.Id });
        }

        public IActionResult Create()
        {
            ViewBag.TaskPriorities = Enum.GetValues(typeof(ToDoTask.Priority))
                                         .Cast<ToDoTask.Priority>()
                                         .ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,TaskPriority,Created,IsCompleted")] ToDoTask toDoTask)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide valid data." });
            }

            try
            {
                if (toDoTask.Created == default)
                {
                    toDoTask.Created = DateTime.Now;
                }

                _context.Add(toDoTask);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Task created successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error on create: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while creating the task." });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.TaskPriorities = Enum.GetValues(typeof(ToDoTask.Priority))
                                         .Cast<ToDoTask.Priority>()
                                         .ToList();
            if (id == null)
            {
                return Json(new { success = false, message = "Task not found." });
            }

            var toDoTask = await _context.ToDoTask.FindAsync(id);
            if (toDoTask == null)
            {
                return Json(new { success = false, message = "Task not found." });
            }

            return View(toDoTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,TaskPriority,Created,IsCompleted")] ToDoTask toDoTask)
        {
            if (id != toDoTask.Id)
            {
                return Json(new { success = false, message = "Task not found." });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please provide valid data." });
            }

            try
            {
                _context.Update(toDoTask);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Task updated successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTaskExists(toDoTask.Id))
                {
                    return Json(new { success = false, message = "Task not found." });
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error on edit: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while updating the task." });
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Task not found." });
            }

            var toDoTask = await _context.ToDoTask.FirstOrDefaultAsync(m => m.Id == id);
            if (toDoTask == null)
            {
                return Json(new { success = false, message = "Task not found." });
            }

            return View(toDoTask);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var toDoTask = await _context.ToDoTask.FindAsync(id);
                if (toDoTask != null)
                {
                    _context.ToDoTask.Remove(toDoTask);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Task deleted successfully." });
                }

                return Json(new { success = false, message = "Task not found." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error on delete: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while deleting the task." });
            }
        }

        public IActionResult ExportToCsv()
        {
            var tasks = _context.ToDoTask.ToList();
            var fileName = "tasks.csv";

            var csv = new StringBuilder();
            csv.AppendLine("Id,Title,Description,TaskPriority,Created,IsCompleted");

            foreach (var task in tasks)
            {
                csv.AppendLine($"{task.Id},{task.Title},{task.Description},{task.TaskPriority},{task.Created},{(task.IsCompleted ? "True" : "False")}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", fileName);
        }

        private bool ToDoTaskExists(int id)
        {
            return _context.ToDoTask.Any(e => e.Id == id);
        }
    }
}
