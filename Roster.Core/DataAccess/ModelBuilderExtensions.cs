using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Roster.Core.Models;
using Roster.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Roster.Core.DataAccess
{
    public static class ModelBuilderExtensions
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RosterContext(
                serviceProvider.GetRequiredService<DbContextOptions<RosterContext>>()))
            {
                // Look for any board games.
                if (await context.Users.AnyAsync())
                {
                    return;   // Data was already seeded
                }
                var pwd = "01234Admin";

                AuthUtil.CreatePasswordHash(pwd, out byte[] passwordHash, out byte[] passwordSalt);
                context.AddRange(
                    new User
                    {
                        Id = 1,
                        Username = "adminovo",
                        CreatedAt = DateTime.Now,
                        Email = "avo.nathan@gmail.com",
                        Role = UserRoles.Admin,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt
                    },
                   new User
                   {
                       Id = 2,
                       Username = "usernew",
                       CreatedAt = DateTime.Now,
                       Email = "nee.nathan@gmail.com",
                       Role = UserRoles.Member,
                       PasswordHash = passwordHash,
                       PasswordSalt = passwordSalt
                   }
                );

                context.SaveChanges();
            }

        }
    }
}

/*
 public class DataGenerator
{
    
    }
}*/