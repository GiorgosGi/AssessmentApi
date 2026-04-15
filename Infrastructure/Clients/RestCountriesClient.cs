using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Clients
{
    public class RestCountriesClient : IRestCountriesClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RestCountriesClient> _logger;
        private const string DefaultApiEndpoint = "v3.1/all?fields=name,capital,borders";

        public RestCountriesClient(HttpClient httpClient, ILogger<RestCountriesClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<RestCountryApiModel>?>(DefaultApiEndpoint, cancellationToken);

                if (response == null || response.Count == 0)
                    return Enumerable.Empty<Country>();

                return response.Select(MapToCountry);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to retrieve countries from external API.");
                throw new InvalidOperationException("Failed to retrieve countries from external API.", ex);
            }
        }

        private static Country MapToCountry(RestCountryApiModel apiModel)
        {
            return new Country
            {
                Name = apiModel.Name?.Common ?? "Unknown",
                Capital = apiModel.Capital?.FirstOrDefault() ?? string.Empty,
                Borders = apiModel.Borders ?? []
            };
        }

        private class RestCountryApiModel
        {
            [JsonPropertyName("name")]
            public CountryName? Name { get; set; }

            [JsonPropertyName("capital")]
            public List<string>? Capital { get; set; }

            [JsonPropertyName("borders")]
            public List<string>? Borders { get; set; }

            public class CountryName
            {
                [JsonPropertyName("common")]
                public string? Common { get; set; }
            }
        }
    }
}