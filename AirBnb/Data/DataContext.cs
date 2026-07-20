using AirBnb.Data;
using Microsoft.EntityFrameworkCore;

namespace AirbnbClone.Data
{
    // Db context organizes the relationships between entities and keeps the DbSets for entities.
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Listing>()
                .HasOne(l => l.host)
                .WithMany(u => u.hosted_listings)
                .HasForeignKey(l => l.hostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Availability>()
                .HasOne(a => a.listing)
                .WithMany(l => l.availabilities)
                .HasForeignKey(a => a.listId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Listing>()
                .HasMany(l => l.reservations)
                .WithOne(r => r.listing)
                .HasForeignKey(r => r.listId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.guest)
                .WithMany(u => u.guest_reservations)
                .HasForeignKey(r => r.guestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.reservation)
                .WithOne(r => r.payment)
                .HasForeignKey<Payment>(p => p.reservationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(rw => rw.reservation)
                .WithMany(r => r.reviews)
                .HasForeignKey(rw => rw.reservationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(rw => rw.reviewer)
                .WithMany(u => u.written_reviews)
                .HasForeignKey(rw => rw.reviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(rw => rw.listing)
                .WithMany(l => l.reviews)
                .HasForeignKey(rw => rw.listingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}