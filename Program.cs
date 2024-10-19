using TaskTracker.Service;

namespace TaskTracker
{
  class Program
  {
    static void Main(string[] args)
    {
        TaskService.Start();
    }
  }
}