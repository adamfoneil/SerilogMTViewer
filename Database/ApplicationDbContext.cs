﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Database.Conventions;
using SerilogBlazor.Abstractions.SavedSearches;

namespace Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options), ISerilogSavedSearches
{	
	public DbSet<Connection> Connections { get; set; }
	public DbSet<SerilogSavedSearch> SerilogSavedSearches { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
	}

	public int SaveChanges(ApplicationUser? user)
	{
		AuditEntities(user);
		return base.SaveChanges();
	}

	public Task<int> SaveChangesAsync(ApplicationUser? user, CancellationToken cancellationToken = default)
	{
		AuditEntities(user);
		return base.SaveChangesAsync(cancellationToken);
	}

	private void AuditEntities(ApplicationUser? user)
	{
		foreach (var entity in ChangeTracker.Entries<BaseTable>())
		{
			switch (entity.State)
			{
				case EntityState.Added:
					entity.Entity.CreatedBy = user?.UserName ?? "system";
					entity.Entity.CreatedAt = LocalDateTime(user?.TimeZoneId);
					break;
				case EntityState.Modified:
					entity.Entity.ModifiedBy = user?.UserName ?? "system";
					entity.Entity.ModifiedAt = LocalDateTime(user?.TimeZoneId);
					break;
			}
		}
	}

	private static DateTime LocalDateTime(string? timeZoneId)
	{
		var now = DateTime.Now;
		if (string.IsNullOrWhiteSpace(timeZoneId)) return now;

		try
		{
			var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
			return TimeZoneInfo.ConvertTime(now, timeZone);
		}
		catch
		{
			return now;
		}
	}
}

public class AppDbFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	private static IConfiguration Config => new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", optional: false)
		.AddUserSecrets("ba97db6d-dd37-488c-8da5-819a88b19558")
		.Build();

	public static string GetConnectionString(string[]? args = null)
	{
		var connectionName = args?.Length == 1 ? args[0] : Config.GetValue<string>("ConnectionName") ?? "DefaultConnection";
		return Config.GetConnectionString(connectionName) ?? throw new Exception($"Connection string '{connectionName}' not found");
	}
	
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var connectionString = GetConnectionString(args);
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
		optionsBuilder.UseNpgsql(connectionString);
		return new ApplicationDbContext(optionsBuilder.Options);
	}
}