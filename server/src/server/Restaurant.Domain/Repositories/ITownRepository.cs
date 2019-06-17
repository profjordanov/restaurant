using Restaurant.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface ITownRepository
    {
        Task<Town> GetByIdAsync(Guid townId);
    }
}