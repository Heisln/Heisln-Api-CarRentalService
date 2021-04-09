using Heisln.Car.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Infrastructure
{
    public class CarRepository : EntityFrameWorkRepository, ICarRepository
    {
        public CarRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public void Add(Domain.Car entity)
        {
            dbContext.Cars.Add(entity);
        }

        public async Task<IEnumerable<Domain.Car>> GetAllAsync()
        {
            return await dbContext.Cars.ToListAsync();
        }

        public async Task<Domain.Car> GetAsync(Guid id)
        {
            return await dbContext.Cars.SingleAsync(car => car.Id == id);
        }

        public void Remove(Domain.Car entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Domain.Car> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
