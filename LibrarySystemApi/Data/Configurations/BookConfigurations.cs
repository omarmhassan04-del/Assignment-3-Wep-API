using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Data.Configurations
{
    public class BookConfigurations : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Title)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(b => b.ISBN)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(b => b.Author)
                   .WithMany(a => a.Books)
                   .HasForeignKey(b => b.AuthorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
