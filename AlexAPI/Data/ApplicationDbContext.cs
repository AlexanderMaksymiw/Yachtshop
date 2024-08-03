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
            modelBuilder.Entity<YachtDetail>().ToTable("Details");
            modelBuilder.Entity<YachtBrochure>().ToTable("Brochures");
            modelBuilder.Entity<Auto>().ToTable("Autos");
            modelBuilder.Entity<Broker>().ToTable("Brokers");
            modelBuilder.Entity<OperatingAreas>().ToTable("OperatingAreas");
            modelBuilder.Entity<OperatingAreaItem>().ToTable("OperatingAreaItems");
            modelBuilder.Entity<CrewMember>().ToTable("CrewMembers");
            modelBuilder.Entity<Crew>().ToTable("Crews");
            modelBuilder.Entity<Photo>().ToTable("Photos");
            modelBuilder.Entity<Video>().ToTable("Videos");
            modelBuilder.Entity<VideoInfo>().ToTable("VideoInfos");
            modelBuilder.Entity<Specifications>().ToTable("Specifications");
            modelBuilder.Entity<Gallery>().ToTable("Galleries");
            modelBuilder.Entity<GalleryItem>().ToTable("GalleryItems");
            modelBuilder.Entity<General>().ToTable("Generals");
            modelBuilder.Entity<Prices>().ToTable("Prices");
            modelBuilder.Entity<PriceTerm>().ToTable("PriceTerms");
            modelBuilder.Entity<KeyFeatureItem>().ToTable("KeyFeatures");
            modelBuilder.Entity<LicenceRegistration>().ToTable("LicenceRegistrations");
            modelBuilder.Entity<OperatingAreaNew>().ToTable("OperatingAreaNews");
            modelBuilder.Entity<Rate>().ToTable("Rates");
            modelBuilder.Entity<SpecialRequest>().ToTable("SpecialRequest");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Yacht> Companies { get; set; }
    }
}
