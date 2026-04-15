using Domain;

namespace Application.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetAllAsync(CancellationToken cancellationToken = default);
        Task SaveAllAsync(List<Country> countries, CancellationToken cancellationToken = default);
    }
}
