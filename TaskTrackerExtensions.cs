namespace TaskTracker.Utilities
{
    class TaskTrackerExtensions
    {
        /// <summary>
        /// Tries to see parse a string into a Guid.
        /// </summary>
        /// <param name="guidString">The <see cref="string"/> you are trying to parse.</param>
        /// <param name="parsedGuid">The <see cref="Guid"/> version of the given string.</param>/// 
        /// <returns>true if the parse operation was successful; otherwise, false. If true, also can use the <see cref="parsedGuid"/> as a variable outside of the method.</returns>
        public static bool TryParseGuid(string guidString, out Guid parsedGuid) 
        {
            return Guid.TryParse(guidString, out parsedGuid);
        }
    }
}