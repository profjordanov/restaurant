using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Persistence.EntityFramework;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Repositories
{
    public class MealTypeRepository : IMealTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MealTypeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<MealType> GetByIdAsync(string typeId) =>
            _dbContext
                .MealTypes
                .SingleOrDefaultAsync(type => type.Id.ToString() == typeId);
    }
}