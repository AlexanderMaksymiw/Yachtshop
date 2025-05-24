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
        public DbSet<Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<SubType> SubTypes { get; set; }
        public DbSet<CharterDeal> Deals { get; set; }
        public DbSet<DealDay> DealDays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
            modelBuilder.Entity<Image>().ToTable("Images");
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
