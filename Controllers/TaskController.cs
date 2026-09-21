using Microsoft.AspNetCore.Mvc;
using c.Models;
using c.Data; // 1. Added this to find your ApplicationDbContext

namespace c.Controllers
{
    // 2. Fixed: Changed 'Controllers' to 'Controller'
    public class TaskController : Controller 
    {
        // 3. Added: Create a private variable to hold our PostgreSQL connection
        private readonly ApplicationDbContext _context;

        // 4. Added: Constructor that injects the database when the controller wakes up
        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Task/Index
        // Displays your task dashboard dashboard list
        [HttpGet]
        public IActionResult Index()
        {
            // Pulls all tasks from PostgreSQL and converts them to a list
            var tasks = _context.TaskItems.ToList(); // Note: change 'SupportTickets' to 'TaskItems' if you renamed the DbSet in your DbContext!
            return View(tasks);
        }

        // GET: /Task/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Update- edit the form
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.TaskItems.Find(id);
            if  (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // Update -save modified changes
        [HttpPost]
        public IActionResult Edit(TaskItem updatedTask)
        {
            if (ModelState.IsValid)
            {
                updatedTask.DueDate = DateTime.SpecifyKind(updatedTask.DueDate, DateTimeKind.Utc);
                // Tell the entity framework to track this modified object and push updates
                _context.TaskItems.Update(updatedTask);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(updatedTask);
        }

        // Delete- remove the task from database
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = _context.TaskItems.Find(id);
            if (task != null)
            {
                _context.TaskItems.Remove(task);//removes the row from PostgreSQL
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // POST: /Task/Create
        [HttpPost]
        public IActionResult Create(TaskItem task)
        {
            // 5. Fixed: Changed !ModelState.IsValid to ModelState.IsValid
            if (ModelState.IsValid)
            {
                // Set our smart defaults automatically on the server
                task.IsCompleted = false;
                task.DueDate = DateTime.UtcNow.AddDays(1);
                task.Category = "General";

                // Save to PostgreSQL
                _context.TaskItems.Add(task); // Note: Change to your matching DbSet name if needed
                _context.SaveChanges();
            
                // 6. Fixed: Changed redirect to point to the actual "Success" method below
                return RedirectToAction("Success");
            }

            // If the model is invalid (e.g. empty fields), return the view to show errors
            return View(task);
        }

        [HttpPost]
        public IActionResult ToggleComplete(int id)
        {
            var task = _context.TaskItems.Find(id);

            if (task != null)
            {
                task.IsCompleted =!task.IsCompleted;

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: /Task/Success
        [HttpGet]
        public IActionResult Success()
        {
            return Content("Your task has been added successfully fahm!!");
        }
    }
}