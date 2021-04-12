using Heisln.Car.Contract;
using Heisln.Car.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Application
{
    public class UserOperationHandler : IUserOperationHandler
    {
        readonly IUserRepository userRepository;

        public UserOperationHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<(string, Guid)> Login(string email, string password)
        {
            var user = await userRepository.GetAsync(email, password);

            var token = JWTTokenGenerator.CreateToken(user);

            return (token, user.Id);
        }

        public async Task<(string, Guid)> Register(string email, string password, string firstName, string lastName, DateTime birthday)
        {
            var newUser = User.Create(email, password, firstName, lastName, birthday);
            userRepository.Add(newUser);
            await userRepository.SaveAsync();

            var user = await userRepository.GetAsync(email, password);
            var token = JWTTokenGenerator.CreateToken(user);
            return (token, newUser.Id);
        }
    }
}
