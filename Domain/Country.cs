namespace Domain
{
    /// <summary>
    /// Represents a country with its identifying information, capital city, and bordering countries.
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Gets or sets the unique identifier for the country.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the capital city of the country.
        /// </summary>
        public string? Capital { get; set; }
        /// <summary>
        /// Gets or sets the list of bordering countries.
        /// </summary>
        public List<string>? Borders { get; set; }
    }
}
