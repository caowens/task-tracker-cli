using System;

namespace TaskTracker.Models
{
    class Todo
    {
        public Guid Id { get; set; } 
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Todo(Guid id, string description)
        {
            Id = id;
            Description = description;
            Status = "todo";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Id}\t\t{Description}\t\t{Status}\t\t{CreatedAt}\t\t{UpdatedAt}\t\t";
        }
    }
}