using Microsoft.EntityFrameworkCore;
using SocialNetwork.Api.Models;

namespace SocialNetwork.Api
{
    public class SocialNetworkContext : DbContext
    {
        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configuracion del Usuario.
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(x => x.Username)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Posts)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.Id);

            modelBuilder.Entity<User>()
               .HasMany(x => x.Commends)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.User.Id);

            //Configuracion de Post
            modelBuilder.Entity<Post>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Post>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Post>()
                .Property(x => x.Content)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .HasMany(x => x.Commends)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.Id);


            //Configuracion de Comentario
            modelBuilder.Entity<Comment>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Comment>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Comment>()
                .Property(x => x.Content)
                .IsRequired();
        }
    }
}
