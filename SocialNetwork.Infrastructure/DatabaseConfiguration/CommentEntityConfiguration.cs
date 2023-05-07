using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Entities;
using System.Reflection.Emit;

namespace SocialNetwork.Infrastructure.DatabaseConfiguration
{
    public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> modelbuilder)
        {
            //Configuracion de Comentario
            modelbuilder
                .HasKey(x => x.Id);

            modelbuilder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelbuilder
                .Property(x => x.Content)
                .IsRequired();
        }
    }
}