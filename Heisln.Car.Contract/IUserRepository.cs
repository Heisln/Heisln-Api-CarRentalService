using Heisln.Car.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Contract
{
    public interface IUserRepository : IRepository<User>
    {
        public static string Secret = "DH7ND98DDSA0"; //Move to settings-file
        Task<User> GetAsync(string username, string password);
    }
}
