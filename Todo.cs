using System;

namespace TaskTracker.Models
{
    class Todo
    {
        public Guid id;
        public string description;
        public string status;
        public DateTime createdAt;
        public DateTime updatedAt;

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