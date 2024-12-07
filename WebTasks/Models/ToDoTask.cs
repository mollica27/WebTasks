using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTasks.Models
{
    [Table("tasks_db")] 
    public class ToDoTask
    {
        public int Id { get; set; }

        [Required] 
        [MaxLength(255)] 
        public string Title { get; set; }

        [MaxLength(1000)] 
        public string? Description { get; set; }

        public Priority TaskPriority { get; set; }

        [Required] 
        public DateTime Created { get; set; }

        public bool IsCompleted { get; set; }

        public enum Priority
        {
            Low,
            Medium,
            High
        }

        public enum Status
        {
            NotStarted,
            InProgress,
            Completed
        }
    }
}
