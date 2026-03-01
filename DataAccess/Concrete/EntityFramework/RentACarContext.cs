using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class RentACarContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=DESKTOP-PH8K0NT\SQLMONSTER;Database=RentACar_V2;Trusted_Connection=true;TrustServerCertificate=True");
        }

        // ─── Auth / RBAC ────────────────────────────────────────────────────────
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<LocationUserRole> LocationUserRoles { get; set; }

        // ─── Customer Domain ────────────────────────────────────────────────────
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CorporateProfile> CorporateProfiles { get; set; }

        // ─── Vehicle ─────────────────────────────────────────────────────────────
        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Gear> Gears { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<CarImage> CarImages { get; set; }

        // ─── Rental / Location ───────────────────────────────────────────────────
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationCity> LocationCities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ─── User ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Email).HasMaxLength(200).IsRequired();
                e.Property(u => u.Status).IsRequired();
            });

            // ─── OperationClaim ───────────────────────────────────────────────────
            modelBuilder.Entity<OperationClaim>(e =>
            {
                e.ToTable("OperationClaims");
                e.HasKey(o => o.Id);
                e.HasIndex(o => o.Name).IsUnique();
                e.Property(o => o.Name).HasMaxLength(100).IsRequired();
            });

            // ─── UserOperationClaim ───────────────────────────────────────────────
            modelBuilder.Entity<UserOperationClaim>(e =>
            {
                e.ToTable("UserOperationClaims");
                e.HasKey(uo => uo.Id);
                e.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(uo => uo.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne<OperationClaim>()
                    .WithMany()
                    .HasForeignKey(uo => uo.OperationClaimId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(uo => new { uo.UserId, uo.OperationClaimId }).IsUnique();
            });

            // ─── LocationUserRole ─────────────────────────────────────────────────
            modelBuilder.Entity<LocationUserRole>(e =>
            {
                e.ToTable("LocationUserRoles");
                e.HasKey(lr => lr.Id);
                e.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(lr => lr.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne<Location>()
                    .WithMany()
                    .HasForeignKey(lr => lr.LocationId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne<OperationClaim>()
                    .WithMany()
                    .HasForeignKey(lr => lr.OperationClaimId)
                    .OnDelete(DeleteBehavior.Restrict);
                // A user can have at most one role per location
                e.HasIndex(lr => new { lr.UserId, lr.LocationId, lr.OperationClaimId }).IsUnique();
            });

            // ─── Customer ─────────────────────────────────────────────────────────
            modelBuilder.Entity<Customer>(e =>
            {
                e.ToTable("Customers");
                e.HasKey(c => c.Id);
                e.HasIndex(c => c.Email).IsUnique();
                e.Property(c => c.Email).HasMaxLength(200).IsRequired();
                e.Property(c => c.PhoneNumber).HasMaxLength(20);
                e.Property(c => c.Address).HasMaxLength(500);
                e.Property(c => c.CustomerType)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();
                e.Property(c => c.CreatedDate).IsRequired();

                // Nullable FK to User — null means guest customer
                e.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(c => c.UserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ─── CorporateProfile ─────────────────────────────────────────────────
            modelBuilder.Entity<CorporateProfile>(e =>
            {
                e.ToTable("CorporateProfiles");
                e.HasKey(cp => cp.Id);
                e.HasIndex(cp => cp.TaxNumber).IsUnique();
                e.HasIndex(cp => cp.CustomerId).IsUnique(); // 1-to-1 with Customer
                e.Property(cp => cp.CompanyName).HasMaxLength(250).IsRequired();
                e.Property(cp => cp.TaxNumber).HasMaxLength(20).IsRequired();
                e.HasOne<Customer>()
                    .WithMany()
                    .HasForeignKey(cp => cp.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── Brand, Color, Fuel, Gear, Segment ───────────────────────────────
            modelBuilder.Entity<Brand>(e => { e.ToTable("Brands"); e.HasKey(b => b.BrandId); });
            modelBuilder.Entity<Color>(e => { e.ToTable("Colors"); e.HasKey(c => c.ColorId); });
            modelBuilder.Entity<Fuel>(e => { e.ToTable("Fuels"); e.HasKey(f => f.FuelId); });
            modelBuilder.Entity<Gear>(e => { e.ToTable("Gears"); e.HasKey(g => g.GearId); });
            modelBuilder.Entity<Segment>(e => { e.ToTable("Segments"); e.HasKey(s => s.SegmentId); });

            // ─── LocationCity (lookup) ────────────────────────────────────────────
            modelBuilder.Entity<LocationCity>(e =>
            {
                e.ToTable("LocationCities");
                e.HasKey(lc => lc.Id);
                e.HasIndex(lc => lc.Name).IsUnique();
                e.Property(lc => lc.Name).HasMaxLength(100).IsRequired();
            });

            // ─── Location ─────────────────────────────────────────────────────────
            modelBuilder.Entity<Location>(e =>
            {
                e.ToTable("Locations");
                e.HasKey(l => l.Id);
                e.Property(l => l.LocationName).HasMaxLength(200).IsRequired();
                e.Property(l => l.Latitude).HasPrecision(9, 6);
                e.Property(l => l.Longitude).HasPrecision(9, 6);

                // FK → LocationCities
                e.HasOne<LocationCity>()
                    .WithMany()
                    .HasForeignKey(l => l.LocationCityId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ─── Car ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<Car>(e =>
            {
                e.ToTable("Cars");
                e.HasKey(c => c.Id);
                e.HasIndex(c => c.PlateNumber).IsUnique();
                e.Property(c => c.PlateNumber).HasMaxLength(20).IsRequired();
                e.Property(c => c.DailyPrice).HasPrecision(18, 2).IsRequired();
                e.Property(c => c.Deposit).HasPrecision(18, 2).IsRequired();
                e.Property(c => c.Description).HasMaxLength(1000);
                e.Property(c => c.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                // FKs
                e.HasOne<Brand>().WithMany().HasForeignKey(c => c.BrandId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Color>().WithMany().HasForeignKey(c => c.ColorId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Location>().WithMany().HasForeignKey(c => c.CurrentLocationId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Fuel>().WithMany().HasForeignKey(c => c.FuelId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Gear>().WithMany().HasForeignKey(c => c.GearId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Segment>().WithMany().HasForeignKey(c => c.SegmentId).OnDelete(DeleteBehavior.Restrict);

                // Concurrency token — prevents two requests from renting at the same time
                e.Property(c => c.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                // Fast availability loopup by location
                e.HasIndex(c => new { c.CurrentLocationId, c.Status })
                    .HasDatabaseName("IX_Cars_LocationStatus");
            });

            // ─── CarImage ─────────────────────────────────────────────────────────
            modelBuilder.Entity<CarImage>(e =>
            {
                e.ToTable("CarImages");
                e.HasKey(ci => ci.CarImageId);
                e.Property(ci => ci.ImagePath).HasMaxLength(500);
                // Note: CarImage uses BrandId/ColorId as reference — no direct Car FK in current design
            });

            // ─── Rental ───────────────────────────────────────────────────────────
            modelBuilder.Entity<Rental>(e =>
            {
                e.ToTable("Rentals");
                e.HasKey(r => r.Id);

                // Financial snapshot columns
                e.Property(r => r.RentedDailyPrice).HasPrecision(18, 2).IsRequired();
                e.Property(r => r.TotalPrice).HasPrecision(18, 2).IsRequired();
                e.Property(r => r.DepositAmount).HasPrecision(18, 2).IsRequired();
                e.Property(r => r.DepositDeductedAmount).HasPrecision(18, 2);

                // Enum → string in DB
                e.Property(r => r.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();
                e.Property(r => r.DepositStatus)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();

                // Concurrency token — prevents two requests from double-booking
                e.Property(r => r.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                // FKs
                e.HasOne<Customer>()
                    .WithMany()
                    .HasForeignKey(r => r.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Car>()
                    .WithMany()
                    .HasForeignKey(r => r.CarId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Location>()
                    .WithMany()
                    .HasForeignKey(r => r.StartLocationId)
                    .OnDelete(DeleteBehavior.Restrict);
                // Second Location FK must use NoAction to avoid multiple cascade paths
                e.HasOne<Location>()
                    .WithMany()
                    .HasForeignKey(r => r.EndLocationId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Index for double-booking guard queries (carId + date range + status lookups)
                e.HasIndex(r => new { r.CarId, r.StartDate, r.EndDate, r.Status })
                    .HasDatabaseName("IX_Rentals_AvailabilityCheck");

                // Active rentals index
                e.HasIndex(r => r.Status)
                    .HasDatabaseName("IX_Rentals_Status");
            });
        }
    }
}
