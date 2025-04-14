using DogusProject.Domain.Entities;
using DogusProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DogusProject.Persistence.SeedData;

public static class SeedData
{
	public static async Task SeedAsync(AppIdentityDbContext context)
	{
		if (!context.Categories.Any())
		{
			var categories = Enumerable.Range(1, 10).Select(i => new Category
			{
				Id = Guid.NewGuid(),
				Name = $"Category {i}"
			}).ToList();

			await context.Categories.AddRangeAsync(categories);
			await context.SaveChangesAsync();
		}

		if (!context.Tags.Any())
		{
			var tags = Enumerable.Range(1, 20).Select(i => new Tag
			{
				Id = Guid.NewGuid(),
				Name = $"Tag {i}"
			}).ToList();

			await context.Tags.AddRangeAsync(tags);
			await context.SaveChangesAsync();
		}

		if (!context.Blogs.Any())
		{
			var blogs = new List<Blog>();
			var rnd = new Random();
			var users = await context.Users.Take(1).ToListAsync();

			if (users.Count == 0)
			{
				Console.WriteLine(" SeedData: Kullanıcı bulunamadı. Önce IdentitySeeder'ın başarılı çalıştığından emin olun.");
				return;
			}

			var tags = await context.Tags.ToListAsync();
			var categories = await context.Categories.ToListAsync();

			if (tags.Count < 3 || categories.Count == 0)
			{
				Console.WriteLine(" SeedData: Tag ya da Category verisi eksik.");
				return;
			}

			for (int i = 1; i <= 30; i++)
			{
				var blog = new Blog
				{
					Id = Guid.NewGuid(),
					Title = $"Sample Blog {i}",
					Content = $"This is the content of blog {i}",
					Status = Domain.Enums.BlogStatus.Published,
					UserId = users.First().Id,
					CategoryId = categories[rnd.Next(categories.Count)].Id,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};

				blog.BlogTags = tags.OrderBy(x => rnd.Next()).Take(3).Select(t => new BlogTag
				{
					BlogId = blog.Id,
					TagId = t.Id
				}).ToList();

				var blogImageCount = rnd.Next(1, 4);

				blog.BlogImages = Enumerable.Range(1, blogImageCount).Select(index =>
				{
					var imageIndex = rnd.Next(1, 6);
					var fileName = $"photo{imageIndex}.jpg";

					return new BlogImage
					{
						Id = Guid.NewGuid(),
						BlogId = blog.Id,
						ImagePath = $"blogs/{fileName}",
						ImageUrl = $"/uploads/blogs/{fileName}"
					};
				}).ToList();

				blogs.Add(blog);
			}

			await context.Blogs.AddRangeAsync(blogs);
			await context.SaveChangesAsync();
		}
	}
}