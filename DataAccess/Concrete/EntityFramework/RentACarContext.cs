using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
   public class RentACarContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-PH8K0NT\SQLMONSTER;Database=RentACar;Trusted_Connection=true;TrustServerCertificate=True");
        }


        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Gear> Gears { get; set; }
        public DbSet<IndividualCustomer> IndividualCustomers { get; set; }
        public DbSet<CorporateCustomer> CorporateCustomers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CorporateUser> CorporateUsers { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<LocationOperationClaim> LocationOperationClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IndividualCustomer>()
                .ToTable("IndividualCustomers")
                .HasKey(ic => ic.CustomerId);

            modelBuilder.Entity<CorporateCustomer>()
                .ToTable("CorporateCustomers")
                .HasKey(cc => cc.CustomerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
