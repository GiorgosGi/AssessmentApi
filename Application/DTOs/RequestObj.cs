namespace Application.DTOs
{
    /// <summary>
    /// Request object containing an array of integers.
    /// </summary>
    public class RequestObj
    {
        /// <summary>
        /// The list of integers to evaluate.
        /// Must contain at least two distinct values.
        /// </summary>
        /// <example>[5, 20, 9, 3, 27]</example>
        public required IEnumerable<int> RequestArrayObj { get; set; }
    }
}
