using Application.Interfaces;
using Application.Services;
using Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests
{
    public class CountryServiceTests
    {
        [Fact]
        public async Task GetCountries_ReturnsDbData()
        {
            var repoMock = new Mock<ICountryRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Country>
            {
                new() { Name = "Testland" }
            });
            var clientMock = new Mock<IRestCountriesClient>();
            var loggerMock = new Mock<ILogger<CountryService>>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new CountryService(repoMock.Object, memoryCache, clientMock.Object, loggerMock.Object);

            var countries = await service.GetCountriesAsync();
            Assert.NotNull(countries);
        }
    }
}