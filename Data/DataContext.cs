using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> User { get; set; }
        
        public DbSet<UserLike> UserLike { get; set; }
        
        public DbSet<Message> Message { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLike>().HasKey(k => new
            {
                k.SourceUserId,
                k.LikedUserId
            });

            modelBuilder.Entity<UserLike>()
                .HasOne<User>(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLike>()
                .HasOne<User>(t => t.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(t => t.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne<User>(m => m.Source)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SourceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne<User>(m => m.Target)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.TargetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}