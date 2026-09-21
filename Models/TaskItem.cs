using System.ComponentModel.DataAnnotations;

namespace c.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title {get; set;} = string.Empty;
        [Required(ErrorMessage = "Please enter the task!!")]
        public string Description {get; set;} = string.Empty;
        public bool IsCompleted {get; set;}
        [Required]
        public DateTime DueDate {get; set;}
        [Required]
        public string Category {get; set;} = string.Empty;
    }
}