using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Entities;
using System.Reflection.Emit;

namespace SocialNetwork.Infrastructure.DatabaseConfiguration
{
    public class PostEntityConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> modelbuilder)
        {
            //Configuracion de Post
            modelbuilder
                .HasKey(x => x.Id);

            modelbuilder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelbuilder
                .Property(x => x.Content)
                .IsRequired();

            modelbuilder
                .HasMany(x => x.Commends)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.Id);
        }
    }
}
