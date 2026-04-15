using Application.Interfaces;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _db;

        public CountryRepository(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<List<Country>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Countries
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task SaveAllAsync(List<Country> countries, CancellationToken cancellationToken = default)
        {
            if (countries == null || !countries.Any())
                return;

            var incomingNames = countries.Select(c => c.Name).ToList();

            var existingNames = await _db.Countries
                .Where(c => incomingNames.Contains(c.Name))
                .Select(c => c.Name)
                .ToListAsync(cancellationToken);

            var existingSet = existingNames.ToHashSet();

            var newCountries = countries
                .Where(c => !existingSet.Contains(c.Name))
                .ToList();

            if (newCountries.Any())
            {
                await _db.Countries.AddRangeAsync(newCountries, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}