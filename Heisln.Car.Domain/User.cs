using System;
using System.Runtime.Serialization;
using System.Text;


namespace Heisln.Car.Domain
{
    public partial class User
    {
        public readonly Guid Id;

        public readonly string Email;

        private string Password;

        public readonly string FirstName;

        public string LastName { get; private set; }

        public readonly DateTime Birthday;

        public User(Guid id, string email, string password, string firstName, string lastName, DateTime birthday)
        {
            Id = id;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }
    }
}
