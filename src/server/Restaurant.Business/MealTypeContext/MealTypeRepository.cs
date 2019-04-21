using Microsoft.EntityFrameworkCore;
using Restaurant.Core.MealTypeContext;
using Restaurant.Domain.Entities;
using Restaurant.Persistence.EntityFramework;
using System.Threading.Tasks;

namespace Restaurant.Business.MealTypeContext
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