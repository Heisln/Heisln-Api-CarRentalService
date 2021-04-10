using System;
using System.Runtime.Serialization;
using System.Text;


namespace Heisln.Car.Domain
{
    public class User
    {
        public readonly Guid Id;

        public readonly string Email;

        public string Password { get; private set; }

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

        public bool CheckPassword(string password)
        {
            return password == Password;
        }

        public static User Create(string email, string password, string firstName, string lastName, DateTime birthday)
        {
            Guid newId = Guid.NewGuid();
            return new User(newId, email, password, firstName, lastName, birthday);
        }
    }
}
