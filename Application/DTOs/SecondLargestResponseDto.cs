namespace Application.DTOs
{
    /// <summary>
    /// Response containing the second largest value.
    /// </summary>
    public class SecondLargestResponseDto
    {
        /// <summary>
        /// The second largest distinct number in the input array.
        /// </summary>
        /// <example>20</example>
        public int Value { get; set; }
    }
}
