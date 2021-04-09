using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heisln.Car.Contract
{
    public interface IRepository<Entity>
    {
        Task<Entity> GetAsync(Guid id);

        Task<IEnumerable<Entity>> GetAllAsync();

        void Add(Entity entity);

        void Remove(Entity entity);

        void Remove(Guid id);

        void RemoveRange(IEnumerable<Entity> entity);

        Task<int> SaveAsync();

    }
}
