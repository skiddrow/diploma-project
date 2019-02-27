using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Context : DbContext
    {
        public Context() : base("DefaultConnection") { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Mileage> Mileages { get; set; }
        public DbSet<Liquid> Liquids { get; set; }
        public DbSet<LiquidType> LiquidTypes { get; set; }
        public DbSet<Fueling> Fuelings { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<ErrorType> ErrorTypes { get; set; }
    }
}