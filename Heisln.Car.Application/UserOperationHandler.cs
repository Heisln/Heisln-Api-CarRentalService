using Heisln.Car.Contract;
using Heisln.Car.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Application
{
    public class UserOperationHandler : IUserOperationHandler
    {
        readonly IRpcClient rpcClient;
        readonly IBookingRepository bookingRepository;

        public UserOperationHandler(IBookingRepository bookingRepository, IRpcClient rpcClient)
        {
            this.bookingRepository = bookingRepository;
        }
        public async Task UpdateUser(User updatedUser)
        {
            var bookings = await bookingRepository.GetBookingsByUser(updatedUser.Id);
            bookings.ToList().ForEach(booking => booking.UpdateUser(updatedUser));
            await bookingRepository.SaveAsync();
        }

        public async Task<User> GetUser(Guid id)
        {
            string message = $"GetUserWithId({id})";
            var response = await rpcClient.Call(message);
            var user = JsonConvert.DeserializeObject<User>(response);
            return user;
        }
    }
}
