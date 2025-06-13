using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AlexAPI.Authentication;
using AlexAPI.Models;

namespace AlexAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Yacht> Yachts { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<KeyFeature> KeyFeatures { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<PreviousName> PreviousNames { get; set; }
        public DbSet<Toy> Toys { get; set; }
        public DbSet<AlexAPI.Models.Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<SubType> SubTypes { get; set; }
        public DbSet<CharterDeal> Deals { get; set; }
        public DbSet<DealDay> DealDays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IMPORTANT: Call base.OnModelCreating first for IdentityDbContext!
            base.OnModelCreating(modelBuilder); // <--- Make sure this line is here and ideally at the top of the method.

            // Link DB Table to Model
            modelBuilder.Entity<Yacht>().ToTable("Yachts");
            modelBuilder.Entity<Specification>().ToTable("Specifications");
            modelBuilder.Entity<Amenity>().ToTable("Amenities");
            modelBuilder.Entity<Award>().ToTable("Awards");
            modelBuilder.Entity<KeyFeature>().ToTable("KeyFeatures");
            modelBuilder.Entity<Equipment>().ToTable("Equipment");
            modelBuilder.Entity<Media>().ToTable("Media");
            modelBuilder.Entity<PreviousName>().ToTable("PreviousNames");
            modelBuilder.Entity<Toy>().ToTable("Toys");
            modelBuilder.Entity<AlexAPI.Models.Image>().ToTable("Images");
            modelBuilder.Entity<Video>().ToTable("Videos");
            modelBuilder.Entity<Location>().ToTable("Locations");
            modelBuilder.Entity<SubType>().ToTable("SubTypes");
            modelBuilder.Entity<CharterDeal>().ToTable("CharterDeals");
            modelBuilder.Entity<DealDay>().ToTable("DealDays");

            // Configure indexes for Yacht
            modelBuilder.Entity<Yacht>()
                .HasIndex(y => y.Name);
            modelBuilder.Entity<Yacht>()
                .HasIndex(y => y.Price);
            modelBuilder.Entity<Yacht>()
                .HasIndex(y => y.OnSale);

            // Configure indexes for Specification
            modelBuilder.Entity<Specification>()
                .HasIndex(s => s.Type);
            modelBuilder.Entity<Specification>()
                .HasIndex(s => s.Length);
            modelBuilder.Entity<Specification>()
                .HasIndex(s => s.Guests);
            modelBuilder.Entity<Specification>()
                .HasIndex(s => s.YearBuilt);
            modelBuilder.Entity<Specification>()
                .HasIndex(s => s.Cabins);
            modelBuilder.Entity<Specification>()
                .HasIndex(s => s.MaxSpeed);
            modelBuilder.Entity<Specification>()
                .HasIndex(s => s.GrossTonnage);
            modelBuilder.Entity<Specification>()
                .HasIndex(s => s.CruisingSpeed);
            modelBuilder.Entity<Specification>()
                .HasIndex(s => s.HullType);
            modelBuilder.Entity<Specification>()
                .HasIndex(s => s.Builder);

            // Configure indexes for Location
            modelBuilder.Entity<Location>()
                .HasIndex(l => l.Name);


            // --- ADD THESE LINES TO ADDRESS DECIMAL WARNINGS ---

            // For CharterDeal
            modelBuilder.Entity<CharterDeal>()
                .Property(cd => cd.Price)
                .HasPrecision(18, 2); // Common for currency, adjust as needed

            // For Location
            modelBuilder.Entity<Location>()
                .Property(l => l.Latitude)
                .HasPrecision(9, 6); // Standard for latitude/longitude (e.g., 123.456789)

            modelBuilder.Entity<Location>()
                .Property(l => l.Longitude)
                .HasPrecision(9, 6); // Standard for latitude/longitude

            // For Specification
            modelBuilder.Entity<Specification>()
                .Property(s => s.Beam)
                .HasPrecision(8, 2); // Adjust precision/scale as per your data needs

            modelBuilder.Entity<Specification>()
                .Property(s => s.CruisingSpeed)
                .HasPrecision(8, 2); // Adjust

            modelBuilder.Entity<Specification>()
                .Property(s => s.Draft)
                .HasPrecision(8, 2); // Adjust

            modelBuilder.Entity<Specification>()
                .Property(s => s.Length)
                .HasPrecision(8, 2); // Adjust

            modelBuilder.Entity<Specification>()
                .Property(s => s.MaxSpeed)
                .HasPrecision(8, 2); // Adjust

            // For Yacht
            modelBuilder.Entity<Yacht>()
                .Property(y => y.Price)
                .HasPrecision(18, 2); // Common for currency, adjust as needed

            // --- END OF NEW LINES ---

            // IMPORTANT: Make sure base.OnModelCreating(modelBuilder); is called somewhere, ideally at the start or end
            // of the OnModelCreating method to ensure Identity-related configurations are applied.
            // I've moved it to the top in this snippet. If it was at the bottom, ensure it remains there or move it to top.
            // Generally, calling base.OnModelCreating first is safer.
        }
    }
}