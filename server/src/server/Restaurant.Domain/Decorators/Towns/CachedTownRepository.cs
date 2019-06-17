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
        private readonly ConcurrentDictionary<Guid, Town> _dict;

        public CachedTownRepository(ITownRepository townRepository)
        {
            _townRepository = townRepository;
            _dict = new ConcurrentDictionary<Guid, Town>();
        }

        public Town GetById(Guid townId) =>
            _dict.GetOrAdd(townId, i => _townRepository.GetById(i));

        public Task<Town> GetByIdAsync(Guid townId) =>
            throw new NotImplementedException();
    }
}