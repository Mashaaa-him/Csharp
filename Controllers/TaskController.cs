using System.IO;
using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CsvHelper.Configuration;
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

        [HttpPost]
public IActionResult UploadCsv(IFormFile csvFile)
{
    if (csvFile == null || csvFile.Length == 0)
    {
        TempData["Error"] = "Please select a valid CSV file first.";
        return RedirectToAction("Index");
    }

    try
    {
        // 1. Configure the parser to ignore capitalization mismatches in column headers
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(), // "title" becomes "title", "Title" becomes "title"
            HeaderValidated = null, // Silences errors if you have extra columns in your spreadsheet
            MissingFieldFound = null
        };

        using (var reader = new StreamReader(csvFile.OpenReadStream()))
        using (var csv = new CsvReader(reader, config))
        {
            var records = csv.GetRecords<TaskItem>().ToList();

            foreach (var task in records)
            {
                // Ensure defaults are enforced
                task.IsCompleted = false;
                if (string.IsNullOrEmpty(task.Category)) task.Category = "Bulk Import";

                // Intercept any date parsed from the CSV and force its Kind to UTC
                if (task.DueDate == DateTime.MinValue)
                {
                    task.DueDate = DateTime.UtcNow.AddDays(1);
                }
                else
                {
                    // If a date was provided, force it to be timezone-aligned for PostgreSQL
                    task.DueDate = DateTime.SpecifyKind(task.DueDate, DateTimeKind.Utc);
                }

                _context.TaskItems.Add(task);
            }

            _context.SaveChanges();
            TempData["Success"] = $"Successfully imported {records.Count} tasks!";
        }
    }
    catch (Exception ex)
    {
        // This will print the precise error message to your dashboard screen so we know exactly why it failed
        TempData["Error"] = $"Import failed: {ex.Message}";
    }

    return RedirectToAction("Index");
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