namespace TaskTracker.Utilities
{
    class TaskTrackerExtensions
    {
        public static bool TryParseGuid(string guidString, out Guid parsedGuid) 
        {
            return Guid.TryParse(guidString, out parsedGuid);
        }
    }
}