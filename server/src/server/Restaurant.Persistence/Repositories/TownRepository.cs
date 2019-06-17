using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Persistence.EntityFramework;
using System;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Repositories
{
    public class TownRepository : ITownRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TownRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Town> GetByIdAsync(Guid townId) =>
            _dbContext
                .Towns
                .SingleOrDefaultAsync(town => town.Id == townId);

        public Town GetById(Guid townId) =>
            _dbContext.Towns.Find(townId);
    }
}