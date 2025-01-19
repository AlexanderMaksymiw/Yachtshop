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
            modelBuilder.Entity<Price>().ToTable("Prices");
            modelBuilder.Entity<Toy>().ToTable("Toys");
            modelBuilder.Entity<Image>().ToTable("Images");
            modelBuilder.Entity<Video>().ToTable("Videos");
            modelBuilder.Entity<Location>().ToTable("Locations");
            modelBuilder.Entity<SubType>().ToTable("SubTypes");
            base.OnModelCreating(modelBuilder);
        }
    }
}
