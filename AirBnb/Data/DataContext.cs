using AirBnb.Data;
using Microsoft.EntityFrameworkCore;

namespace AirbnbClone.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<ListingImage> ListingImages { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Listing>()
                .HasOne(l => l.hostId)
                .WithMany(u => u.HostedListings)
                .HasForeignKey(l => l.hostId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<ListingImage>()
                .HasOne(li => li.Listing)
                .WithMany(l => l.ListingImages)
                .HasForeignKey(li => li.listId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Availability>()
                .HasOne(a => a.Listing)
                .WithMany(l => l.Availabilities)
                .HasForeignKey(a => a.listId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Listing)
                .WithMany(l => l.Reservations)
                .HasForeignKey(r => r.listId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Guest)
                .WithMany(u => u.GuestReservations)
                .HasForeignKey(r => r.guestId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Reservation)
                .WithOne(r => r.Payment)
                .HasForeignKey<Payment>(p => p.reservationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(rw => rw.Reservation)
                .WithMany(r => r.Reviews)
                .HasForeignKey(rw => rw.reservationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(rw => rw.Reviewer)
                .WithMany(u => u.WrittenReviews)
                .HasForeignKey(rw => rw.reviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(rw => rw.Listing)
                .WithMany(l => l.Reviews)
                .HasForeignKey(rw => rw.listingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}