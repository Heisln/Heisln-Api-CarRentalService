using Heisln.Car.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Application
{
    public interface IUserOperationHandler
    {
        Task UpdateUser(User updatedUser);
        Task<User> GetUser(Guid id);
    }
}
