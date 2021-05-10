using Heisln.Api.Models;
using Heisln.Api.Security;
using Heisln.Car.Api.Attributes;
using Heisln.Car.Application;
using Heisln.Car.Application.BackgroundServices;
using Heisln.Car.Application.Configurations;
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
using System.Text.Json.Serialization;
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Heisln Carrental Service", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                    }
                });
                c.SchemaFilter<EnumSchemaFilter>();
            });

            var serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
            var serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqConfiguration>();
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt => {
                var key = Encoding.ASCII.GetBytes(IBookingRepository.Secret);

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
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ICarRepository, CarRepository>();

            //DI
            services.AddScoped<ICarOperationHandler, CarOperationHandler>();
            services.AddScoped<IBookingOperationHandler, BookingOperationHandler>();
            services.AddScoped<IUserOperationHandler, UserOperationHandler>();
            services.AddScoped<ICurrencyConverterHandler, CurrencyConverterHandler>();
            services.AddScoped<IRpcClient, RpcClient>();

            //Configure Database
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseMySQL(Configuration.GetSection("Database")["ConnectionString"]);
            });

            if (serviceClientSettings.Enabled)
            {
                services.AddHostedService<UserListener>();
            }
            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (true)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Heisln Carrental Service v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

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

                if (true)
                {
                    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

                    try
                    {
                        var dbContext = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                        dbContext.Database.Migrate();

                        dbContext.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0;");
                        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Cars");
                        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Bookings");
                        dbContext.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 1;");


                        var carA = new Car.Domain.Car(Guid.NewGuid(), "BWM", "43e", 77, 2.0, 1.0);
                        var carB = new Car.Domain.Car(Guid.NewGuid(), "BWM", "X7", 77, 2.0, 1.0);

                        var carRepository = serviceScope.ServiceProvider.GetRequiredService<ICarRepository>();
                        carRepository.Add(carA);
                        carRepository.Add(carB);
                        carRepository.SaveAsync().Wait();

                        var userA = new Car.Domain.User(Guid.NewGuid(), "mail@mail.test", "pwd", "Max", "Mustermann", DateTime.Today);
                        var userB = new Car.Domain.User(Guid.NewGuid(), "mail1@mail.test", "pwd", "Sabine", "Sicher", DateTime.Today);

                        var bookingA = Car.Domain.Booking.Create(carA, userA, DateTime.Now, DateTime.Now);
                        var bookingB = Car.Domain.Booking.Create(carB, userA, DateTime.Now, DateTime.Now);

                        var bookingRepository = serviceScope.ServiceProvider.GetRequiredService<IBookingRepository>();
                        bookingRepository.Add(bookingA);
                        bookingRepository.Add(bookingB);

                        bookingRepository.SaveAsync().Wait();
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
