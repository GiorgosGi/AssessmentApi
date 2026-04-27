using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly IRestCountriesClient _client;
        private readonly ILogger<CountryService> _logger;

        private const string CacheKey = "countries";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);
        private static readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

        public CountryService(
            ICountryRepository repository,
            IMemoryCache cache,
            IRestCountriesClient client,
            ILogger<CountryService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken cancellationToken = default)
        {
            if (_cache.TryGetValue(CacheKey, out List<Country>? cached) && cached != null)
            {
                _logger.LogInformation(
                    "Countries fetched from Cache. Count: {Count}.", cached.Count);

                return cached.Select(MapToDto);
            }

            await _cacheLock.WaitAsync(cancellationToken);
            try
            {
                // Double-check after acquiring the lock
                if (_cache.TryGetValue(CacheKey, out cached) && cached != null)
                {
                    _logger.LogInformation(
                        "Countries fetched from Cache (after lock). Count: {Count}.", cached.Count);
                    return cached.Select(MapToDto);
                }

                _logger.LogDebug("Cache miss for {CacheKey}", CacheKey);

                var dbCountries = await _repository.GetAllAsync(cancellationToken);

                if (dbCountries.Count != 0)
                {
                    var list = dbCountries.ToList();

                    _cache.Set(CacheKey, list, CacheDuration);

                    _logger.LogInformation(
                        "Countries fetched from Database. Count: {Count}.", list.Count);

                    return list.Select(MapToDto);
                }

                _logger.LogWarning(
                    "No countries found in Database. Falling back to ExternalApi");

                var apiCountries = (await _client.GetAllCountriesAsync(cancellationToken)).ToList();

                if (apiCountries.Count != 0)
                {
                    await _repository.SaveAllAsync(apiCountries, cancellationToken);
                    _cache.Set(CacheKey, apiCountries, CacheDuration);

                    _logger.LogInformation(
                        "Countries fetched from ExternalApi. Count: {Count}.", apiCountries.Count);
                }
                else
                {
                    _logger.LogWarning(
                        "No countries returned from ExternalApi");
                }

                return apiCountries.Select(MapToDto);
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        private static CountryDto MapToDto(Country country)
        {
            return new CountryDto
            {
                Name = country.Name,
                Capital = country.Capital,
                Borders = country.Borders
            };
        }
    }
}