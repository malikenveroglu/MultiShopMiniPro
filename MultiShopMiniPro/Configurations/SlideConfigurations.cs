using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiShopMiniPro.Models;

namespace MultiShopMiniPro.DAL.Configurations
{
    public class SlideConfiguration : IEntityTypeConfiguration<Slide>
    {
        public void Configure(EntityTypeBuilder<Slide> builder)
        {
            builder.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.SubTitle)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.Image)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(s => s.Order)
                .IsRequired();

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.Property(s => s.IsDeleted)
                .IsRequired();
        }
    }
}