namespace Application.DTOs
{
    /// <summary>
    /// Represents a country.
    /// </summary>
    public class CountryDto
    {
        /// <summary>
        /// The name of the country.
        /// </summary>
        /// <example>Mexico</example>
        public string? Name { get; set; }
        /// <summary>
        /// The capital of the country.
        /// </summary>
        /// <example>Mexico City</example>
        public string? Capital { get; set; }
        /// <summary>
        /// The borders of the country.
        /// </summary>
        /// <example>["BLZ", "GTM", "USA"]</example>
        public List<string>? Borders { get; set; }
    }
}
