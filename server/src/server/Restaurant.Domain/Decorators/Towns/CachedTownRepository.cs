using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Restaurant.Domain.Decorators.Towns
{
    public class CachedTownRepository : ITownRepository
    {
        private readonly ITownRepository _townRepository;

        public CachedTownRepository(ITownRepository townRepository)
        {
            _townRepository = townRepository;
        }

        public Task<Town> GetByIdAsync(Guid townId)
        {
            return new Task<Town>(() => new Town());
        }
    }
}