using Heisln.Api.Controllers;
using Heisln.Car.Application;
using Heisln.Car.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Heisln.Api.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Linq;

namespace Heisln.ApiTest
{
    public class UserControllerTest : IClassFixture<DbContextFixture>
    {
        UserController userController;
        public UserControllerTest(DbContextFixture dbContextFixture)
        {
            DatabaseContext databaseContext = dbContextFixture.DatabaseContext;
            userController = new UserController(
                new UserOperationHandler(
                    userRepository: new UserRepository(databaseContext)
                ),
                new BookingOperationHandler(
                    bookingRepository: new BookingRepository(databaseContext), 
                    userRepository: new UserRepository(databaseContext)
                )
            );
        }

        public static readonly IEnumerable<object[]> userInformation = new List<object[]>
        {
            new object[] { "John", "Doe", DateTime.Now, "john.doe@gmail.com", "asdfg123"},
            new object[] { "Max", "Mustermann", new DateTime(2000, 1, 1), "max.mustermann@gmail.at", "popcorn"}
        };

        [Theory]
        [MemberData(nameof(userInformation))]
        public async Task RegistrateUser_GivenANewUser_WhenRegistrateUser_ThenUserIsRegisteredGetsJWT(string firstname, string lastname, DateTime birthday, string email, string password)
        {
            // Given
            var newUser = CreateUser(firstname, lastname, birthday, email, password);

            // When
            var authenticationResponse = await userController.RegistrateUser(newUser);

            // Then
            authenticationResponse.Token.Should().BeOfType<string>("because the JWT is encoded as string.").And.Should().NotBeNull();
        }

        // To Do: Currently Backend saves every User even if they are faulty like missing a information
        //public static readonly IEnumerable<object[]> incompleteUserData = new List<object[]>
        //{
        //    new object[] { "", "", new DateTime(), "", ""},
        //    new object[] { "", "", new DateTime(), "max.mustermann@gmail.at", "popcorn"}
        //};

        //[Theory]
        //[MemberData(nameof(incompleteUserData))]
        //public async Task RegistrateUser_GivenEmptyUser_WhenRegistrateUser_ThenItShouldFail(string firstname, string lastname, DateTime birthday, string email, string password)
        //{
        //    var newUser = CreateUser(firstname, lastname, birthday, email, password);
        //    await Assert.ThrowsAnyAsync<Exception>(() => userController.RegistrateUser(newUser));
        //}

        public User CreateUser(string firstname, string lastname, DateTime birthday, string email, string password)
        {
            return new User()
            {
                FirstName = firstname,
                LastName = lastname,
                Birthday = birthday,
                Email = email,
                Password = password
            };
        }

        [Theory]
        [InlineData("hans.peter@yahoo.at", "asdfg")]
        [InlineData("gustav.nimmersatt@yahoo.at", "asdfg123")]
        public async Task UserLogin_GivenExistingUserCredentials_WhenUserTriesToLogin_ThenUserIsLoggedInAndGetsJWT(string email, string password)
        {
            // Given
            var authenticationRequest = CreateAuthenticationRequest(email, password);

            // When
            var authenticationResponse = await userController.UserLogin(authenticationRequest);

            // Then
            authenticationResponse.Token.Should().BeOfType<string>("because the JWT is encoded as string.").And.Should().NotBeNull();
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("hans.peter@yahoo.at", "")]
        [InlineData("", "asdfg")]
        [InlineData("not.existing@email.com", "asdfg")]
        public async Task UserLogin_GivenEmptyUserCredentials_WhenUserTriesToLogin_ThenItShouldFail(string email, string password)
        {
            var authenticationRequest = CreateAuthenticationRequest(email, password);
            await Assert.ThrowsAnyAsync<Exception>(() => userController.UserLogin(authenticationRequest));
        }


        // To Do: Method Extract
        public AuthenticationRequest CreateAuthenticationRequest(string email, string password)
        {
            return new AuthenticationRequest()
            {
                Email = email,
                Password = password
            };
        }

        [Theory]
        [InlineData("hans.peter@yahoo.at", "asdfg")]
        public async Task GetBooking_GivenARegisteredUserAndBookingId_WhenUserTriesToFetchASpecificBooking_ThenReceiveTheSpecificBooking(string email, string password)
        {
            // Given
            var authenticationResponse = await userController.UserLogin(CreateAuthenticationRequest(email, password));
            var bookings = await userController.GetBookings(authenticationResponse.UserId);
            var expectedBooking = bookings.ToList().First();

            // When
            var specificBooking = await userController.GetBooking(authenticationResponse.UserId, (Guid)expectedBooking.Id);

            // Then
            specificBooking.Id.Should().Be(expectedBooking.Id, "because the User fetch this booking.");
        }

        [Theory]
        [InlineData("hans.peter@yahoo.at", "asdfg")]
        [InlineData("gustav.nimmersatt@yahoo.at", "asdfg123")]
        public async Task GetBooking_GivenARegisteredUser_WhenUserTriesToFetchNotExistingBooking_ThenItShouldFail(string email, string password)
        {
            var authenticationResponse = await userController.UserLogin(CreateAuthenticationRequest(email, password));

            await Assert.ThrowsAnyAsync<Exception>(() => userController.GetBooking(authenticationResponse.UserId, Guid.NewGuid()));
        }

        [Theory]
        [InlineData("hans.peter@yahoo.at", "asdfg", 3)]
        [InlineData("gustav.nimmersatt@yahoo.at", "asdfg123", 0)]
        public async Task GetBookings_GivenARegisteredUser_WhenUserTriesToFetchAllBookings_ThenTheCountShouldBe(string email, string password, int count)
        {
            // Given
            var authenticationResponse = await userController.UserLogin(CreateAuthenticationRequest(email, password));

            // When
            var bookings = await userController.GetBookings(authenticationResponse.UserId);

            // Then
            bookings.Should().HaveCount(count, $"because the User booked {count} cars!");
        }
    }
}
