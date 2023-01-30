using GarageCooperative.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageCooperative.DataBase
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Cooperative> Cooperatives { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Garage> Garages { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<GarageCooperative.Models.Type> Types { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GarageCooperative;Trusted_Connection=True;");
            // optionsBuilder.UseSqlServer("Server=DESKTOP-KIV92L3;Database=GarageCooperative;Trusted_Connection=True;Encrypt=False;");
            optionsBuilder.UseSqlServer("Server=DESKTOP-I75L3P7;Database=GarageCooperative;Trusted_Connection=True;Encrypt=False;");
        }
    }
}
