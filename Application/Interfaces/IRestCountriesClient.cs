using Domain;

namespace Application.Interfaces
{
    public interface IRestCountriesClient
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync(CancellationToken cancellationToken = default);
    }
}
