using System;
using System.Threading.Tasks;

namespace Heisln.Car.Application
{
    public interface IUserOperationHandler
    {
        Task<(string, Guid)> Login(string username, string password);

        Task<string> Register(string email, string password, string firstName, string lastName, DateTime birthday);
    }
}
