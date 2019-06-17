using Restaurant.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface ITownRepository
    {
        Town GetById(Guid townId);

        Task<Town> GetByIdAsync(Guid townId);
    }
}