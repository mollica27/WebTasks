using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTasks.Data;
using WebTasks.Models;
using System.Text;

namespace WebTasks.Controllers
{
    public class ToDoTasksController : Controller
    {
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
                return NotFound();
            }

            var toDoTask = await _context.ToDoTask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoTask == null)
            {
                return NotFound();
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

            var task = _context.ToDoTask
                               .FirstOrDefault(t => t.Title.Contains(query));

            if (task == null)
            {
               
                TempData["Message"] = "Tarefa não encontrada.";
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
            if (ModelState.IsValid)
            {
                if (toDoTask.Created == default)
                {
                    toDoTask.Created = DateTime.Now;
                }

                _context.Add(toDoTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoTask);
        }

        public async Task<IActionResult> Edit(int? id)
        {
             ViewBag.TaskPriorities = Enum.GetValues(typeof(ToDoTask.Priority)) 
                                         .Cast<ToDoTask.Priority>() 
                                         .ToList();
            if (id == null)
            {
                return NotFound();
            }
            var toDoTask = await _context.ToDoTask.FindAsync(id);
            if (toDoTask == null)
            {
                return NotFound();
            }
            return View(toDoTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,TaskPriority,Created,IsCompleted")] ToDoTask toDoTask)
        {
            if (id != toDoTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoTaskExists(toDoTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toDoTask);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoTask = await _context.ToDoTask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            return View(toDoTask);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoTask = await _context.ToDoTask.FindAsync(id);
            if (toDoTask != null)
            {
                _context.ToDoTask.Remove(toDoTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoTaskExists(int id)
        {
            return _context.ToDoTask.Any(e => e.Id == id);
        }
    }
}
