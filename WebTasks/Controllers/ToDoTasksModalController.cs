using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTasks.Data;
using WebTasks.Models;

namespace WebTasks.Controllers
{
    [Route("modals/[controller]")]
    public class ToDoTasksModalController : Controller
    {
        private readonly WebTasksContext _context;

        public ToDoTasksModalController(WebTasksContext context)
        {
            _context = context;
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.TaskPriorities = Enum.GetValues(typeof(ToDoTask.Priority))
                                         .Cast<ToDoTask.Priority>()
                                         .ToList();
            return PartialView("~/Views/ToDoTasks/_CreateModal.cshtml");
        }

        [HttpPost("Create")]
        
        public async Task<IActionResult> Create(ToDoTask task)
        {
            if (!ModelState.IsValid)
            {
                
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }

                return Json(new { success = false, message = "Please provide valid data." });
            }

            try
            {
                task.Created = task.Created == default ? DateTime.Now : task.Created;
                _context.Add(task);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Task created successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during task creation: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while creating the task." });
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.TaskPriorities = Enum.GetValues(typeof(ToDoTask.Priority))
                                         .Cast<ToDoTask.Priority>()
                                         .ToList();

            var toDoTask = await _context.ToDoTask.FindAsync(id);
            if (toDoTask == null)
            {
                
                return NotFound();
            }

            return PartialView("~/Views/ToDoTasks/_EditModal.cshtml", toDoTask);
        }

        [HttpPost("Edit")]
        
        public async Task<IActionResult> Edit(ToDoTask task)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }

                return Json(new { success = false, message = "Please provide valid data.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
            }

            try
            {
                task.IsCompleted = (Request.Form["IsCompleted"] == "on");

                _context.Update(task);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Task updated successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTaskExists(task.Id))
                {
                   
                    return Json(new { success = false, message = "Task not found." });
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during task update: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while updating the task." });
            }
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var toDoTask = await _context.ToDoTask.FindAsync(id);
            if (toDoTask == null)
            {
               
                return NotFound();
            }

            return PartialView("~/Views/ToDoTasks/_DeleteModal.cshtml", toDoTask);
        }

        [HttpPost("DeleteConfirmed")]
        
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
                Console.WriteLine($"Error during task deletion: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while deleting the task." });
            }
        }

        private bool ToDoTaskExists(int id)
        {
            return _context.ToDoTask.Any(e => e.Id == id);
        }
    }
}
