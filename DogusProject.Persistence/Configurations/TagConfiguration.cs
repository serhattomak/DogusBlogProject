using DogusProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DogusProject.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
	public void Configure(EntityTypeBuilder<Tag> builder)
	{
		builder.HasKey(t => t.Id);
		builder.Property(t => t.Name).IsRequired().HasMaxLength(50);

		builder.HasMany(t => t.BlogTags)
			.WithOne(bt => bt.Tag)
			.HasForeignKey(bt => bt.TagId);

		builder.ToTable("Tags");
	}
}