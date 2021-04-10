using Heisln.Api.Models;
using Heisln.Api.Security;
using Heisln.Car.Application;
using Heisln.Car.Contract;
using Heisln.Car.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Heisln.Car.Api", Version = "v1" });
            });

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt => {
                var key = Encoding.ASCII.GetBytes(IUserRepository.Secret);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

            //DI - Database
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ICarRepository, CarRepository>();

            //DI
            services.AddScoped<ICarOperationHandler, CarOperationHandler>();
            services.AddScoped<IUserOperationHandler, UserOperationHandler>();

            //Configure Database
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseMySQL(Configuration.GetSection("Database")["ConnectionString"]);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Heisln.Car.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Seed_Test_Data
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var databaseContext = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();

                if (env.IsDevelopment())
                {
                    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

                    try
                    {
                        var dbContext = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                        dbContext.Database.Migrate();

                        dbContext.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0;");
                        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Cars");
                        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Bookings");
                        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Users");
                        dbContext.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 1;");


                        var carA = new Car.Domain.Car(Guid.NewGuid(), "BWM", "43e", 77, 2.0, 1.0);
                        var carB = new Car.Domain.Car(Guid.NewGuid(), "BWM", "X7", 77, 2.0, 1.0);

                        var carRepository = serviceScope.ServiceProvider.GetRequiredService<ICarRepository>();
                        carRepository.Add(carA);
                        carRepository.Add(carB);
                        carRepository.SaveAsync().Wait();

                        var userA = new Car.Domain.User(Guid.NewGuid(), "mail@mail.test", "pwd", "Max", "Mustermann", DateTime.Today);
                        var userB = new Car.Domain.User(Guid.NewGuid(), "mail@mail.test", "pwd", "Sabine", "Sicher", DateTime.Today);

                        var userRepository = serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();
                        userRepository.Add(userA);
                        userRepository.Add(userB);
                        userRepository.SaveAsync().Wait();

                    }
                    catch (Exception e)
                    {
                        logger.LogWarning(e, "Could not create test data!");
                    }
                }
            }
            #endregion
        }


    }
}
