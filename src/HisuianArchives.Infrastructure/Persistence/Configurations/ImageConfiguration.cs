using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Url).IsRequired();

        builder.HasOne(i => i.Owner)
               .WithMany()
               .HasForeignKey(i => i.OwnerId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade); 
    }
}