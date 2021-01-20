using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Roster.Core.Models;

namespace Roster.Core.DataAccess
{
    public class RosterContext : DbContext 
    {
        public RosterContext(DbContextOptions<RosterContext> options)
     : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Rosters> Rosters { get; set; }
        public DbSet<Color> Colors { get; set; }


        // Todo : Seed DB with admin user credentials

    }
}
