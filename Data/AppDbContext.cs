using ASP.NET.SQLSERVER.JWT.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.SQLSERVER.JWT.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Message> Messages { get; set; }
<<<<<<< HEAD

        public DbSet<Image> Images { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<UserConversation> UserConversations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Blog → User (1-n)
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Blog → Image (1-1)
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Image)
                .WithMany()
                .HasForeignKey(b => b.ImageUrl)
                .OnDelete(DeleteBehavior.Restrict);

            // User → Image (1-1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Image)
                .WithMany()
                .HasForeignKey(u => u.ImageUrl)
                .OnDelete(DeleteBehavior.Restrict);

            // Message → Blog (n-1)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Blog)
                .WithMany(b => b.Messages)
                .HasForeignKey(m => m.BlogId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message → User (n-1)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            // Message → Conversation (n-1)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Restrict);
            // User → Blogs (1-n)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Blogs)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserConversation>()
            .HasKey(uc => new { uc.UserId, uc.ConversationId });

            modelBuilder.Entity<UserConversation>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserConversations)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserConversation>()
                .HasOne(uc => uc.Conversation)
                .WithMany(c => c.UserConversations)
                .HasForeignKey(uc => uc.ConversationId);
        }
=======
>>>>>>> 29b717529d48c47e43a0d479a6466c2d2b48c4df
    }
}
