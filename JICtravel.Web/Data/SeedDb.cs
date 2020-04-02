using JICtravel.Common.Enums;
using JICtravel.Web.Data.Entities;
using JICtravel.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JICtravel.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext dataContext, IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1000", "Daniel Dario", "Cano Peña", "ddcp10@gmail.com", "311 389 1325", UserType.Admin);
            var user1 = await CheckUserAsync("1010", "Dario", "Cano", "danieldario_01@hotmail.com", "319 524 2117", UserType.Slave);
            var user2 = await CheckUserAsync("1020", "Dani", "Peña", "danielcano198367@correo.itm.edu.co", "322 234 4798", UserType.Slave);
            await CheckTripsAsync(user1, user2);
        }

        private async Task<SlaveEntity> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            UserType userType)
        {
            SlaveEntity user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new SlaveEntity
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Document = document,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Slave.ToString());
        }


        private async Task CheckTripsAsync(
            SlaveEntity user1,
            SlaveEntity user2
)
        {
            _dataContext.Trips.Add(new TripEntity
            {
                Slave = user1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(2),
                CityVisited = "Bogotá",
                TripDetails = new List<TripDetailEntity>
                    {
                        new TripDetailEntity
                        {
                            StartDate = DateTime.UtcNow,
                            Expensive = 580000,
                            ExpensivesType = new List<ExpensiveTypeEntity>
                            {
                                new ExpensiveTypeEntity
                                {
                                    ExpensiveType = "Hotel"
                                }
                            }
                        },
                        new TripDetailEntity
                        {
                            StartDate = DateTime.UtcNow,
                            Expensive = 390000,
                            ExpensivesType = new List<ExpensiveTypeEntity>
                            {
                                new ExpensiveTypeEntity
                                {
                                    ExpensiveType = "Alimentación"
                                }
                            }
                        },
                        new TripDetailEntity
                        {
                            StartDate = DateTime.UtcNow,
                            Expensive = 60000,
                            ExpensivesType = new List<ExpensiveTypeEntity>
                            {
                                new ExpensiveTypeEntity
                                {
                                    ExpensiveType = "Transporte"
                                }
                            }
                        },
                    }

            });

            _dataContext.Trips.Add(new TripEntity
            {
                Slave = user2,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(2),
                CityVisited = "Bogotá",
                TripDetails = new List<TripDetailEntity>
                    {
                        new TripDetailEntity
                        {
                            StartDate = DateTime.UtcNow,
                            Expensive = 250000,
                            ExpensivesType = new List<ExpensiveTypeEntity>
                            {
                                new ExpensiveTypeEntity
                                {
                                    ExpensiveType = "Hotel"
                                }
                            }
                        },
                        new TripDetailEntity
                        {
                            StartDate = DateTime.UtcNow,
                            Expensive = 100000,
                            ExpensivesType = new List<ExpensiveTypeEntity>
                            {
                                new ExpensiveTypeEntity
                                {
                                    ExpensiveType = "Alimentación"
                                }
                            }
                        },
                        new TripDetailEntity
                        {
                            StartDate = DateTime.UtcNow,
                            Expensive = 35000,
                            ExpensivesType = new List<ExpensiveTypeEntity>
                            {
                                new ExpensiveTypeEntity
                                {
                                    ExpensiveType = "Transporte"
                                }
                            }
                        }
                    }

            });

            await _dataContext.SaveChangesAsync();
        }
    }
}
