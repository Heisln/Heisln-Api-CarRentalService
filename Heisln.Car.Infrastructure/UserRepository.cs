using Heisln.Car.Contract;
using Heisln.Car.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Infrastructure
{
    public class UserRepository : EntityFrameWorkRepository, IUserRepository
    {
        public UserRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public void Add(User entity)
        {
            dbContext.Users.Add(entity);
        }

        public async Task<User> GetAsync(string email, string password)
        {
            var user = await dbContext.Users.SingleAsync(user => user.Email == email && user.Password == password);
            return user;
        }

        public Task<User> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Remove(User entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<User> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
