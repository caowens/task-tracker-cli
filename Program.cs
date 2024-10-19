using TaskTracker.Service;

namespace TaskTracker
{
  class Program
  {
    static void Main(string[] args)
    {
        TaskService service = new TaskService();
        service.Start();
    }
  }
}