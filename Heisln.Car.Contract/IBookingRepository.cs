using Heisln.Car.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Contract
{
    public interface IBookingRepository : IRepository<Booking>
    {
        public static string Secret = "DH7ND98DDSA13210FDSEKFJFJF543KJCKKOP543FKOPFLPÜF543KFKJKLRIFIORKL6894829";
        Task<IEnumerable<Booking>> GetBookingsByUser(Guid userId);
    }
}
