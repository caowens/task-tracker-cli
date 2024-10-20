using System;

namespace TaskTracker.Models
{
    class Todo
    {
        public Guid id { get; set; } 
        public string description { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public Todo(Guid Id, string Description)
        {
            id = Id;
            description = Description;
            status = "todo";
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{id}\t\t{description}\t\t{status}\t\t{createdAt}\t\t{updatedAt}\t\t";
        }
    }
}