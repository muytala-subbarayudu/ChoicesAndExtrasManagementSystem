using ChoicesExtrasManagement.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace ChoicesExtrasManagement.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ChoicesExtrasManagementDbContext>();

                context.Database.EnsureCreated();

                //Locations
                if (!context.Locations.Any())
                {
                    context.Locations.AddRange(new List<Location>()
                    {
                        new Location()
                        {
                            Name = "Leicester"
                        },
                        new Location()
                        {
                            Name = "Rugby"
                        },
                        new Location()
                        {
                            Name = "London"
                        }
                    });
                    context.SaveChanges();
                }

                //Plot Types
                if (!context.PlotTypes.Any())
                {
                    context.PlotTypes.AddRange(new List<PlotType>()
                    {
                        new PlotType()
                        {
                            Name = "Bronze",
                            Description = "Basic level home",
                            Amount = 100000
                        },
                        new PlotType()
                        {
                            Name = "Silver",
                            Description = "Intermediate level home",
                            Amount = 200000
                        },
                        new PlotType()
                        {
                            Name = "Gold",
                            Description = "Standard level home",
                            Amount = 300000
                        },
                        new PlotType()
                        {
                            Name = "Platinum",
                            Description = "Advanced level home",
                            Amount = 400000
                        }
                    });
                    context.SaveChanges();
                }

                //Room Types
                if (!context.Rooms.Any())
                {
                    context.Rooms.AddRange(new List<Room>()
                    {
                        new Room()
                        {
                            Name = "Bedroom",
                            Description = "Individual sleeping spaces for family members or guests",
                        },
                        new Room()
                        {
                            Name = "Kitchen",
                            Description = "A separate room designated for formal dining",
                        },
                        new Room()
                        {
                            Name = "Bathroom",
                            Description = "Rooms with facilities for personal hygiene, including toilets, sinks, and showers or bathtubs",
                        },
                        new Room()
                        {
                            Name = "Living Room",
                            Description = "The main communal area of the house for relaxation, entertainment, and socializing",
                        }
                    });
                    context.SaveChanges();
                }

                //Plot , Project , Location 
                if (!context.Plots.Any())
                {
                    context.Plots.AddRange(new List<Plot>()
                    {
                        new Plot()
                        {
                            PlotType = new PlotType()
                            {
                                Name = "Diamond",
                                Description = "Premium level home",
                                Amount = 500000
                            },
                            Project = new Project()
                            {
                                Name = "Blue Homes",
                                Description = "All range plots at affordable prices",
                                Location = new Location()
                                {
                                    Name = "Nottingham"
                                }                              
                            },
                            AppUserId = "83336823-c5a0-4007-b630-e1b32592c1f0"
                        }
                    });
                    context.SaveChanges();
                }

            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "mutyalasubbarayudu@outlook.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "Mutyala",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Admin123#");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "mutyalasubbu95@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "Subbu",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAppUser, "User123#");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
