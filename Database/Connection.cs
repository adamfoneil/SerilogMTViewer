using Database.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database;

public class Connection : BaseTable
{
	public int OwnerUserId { get; set; }
	public string ApplicationName { get; set; } = default!;
	public string Endpoint { get; set; } = default!;
	public string HeaderSecret { get; set; } = default!;

	public ApplicationUser? OwnerUser { get; set; }
}

public class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
{
	public void Configure(EntityTypeBuilder<Connection> builder)
	{
		builder.Property(e => e.ApplicationName).HasMaxLength(100).IsRequired();
		builder.HasIndex(e => new { e.OwnerUserId, e.ApplicationName }).IsUnique();
		builder.Property(e => e.Endpoint).HasMaxLength(200).IsRequired();
		builder.Property(e => e.HeaderSecret).HasMaxLength(100).IsRequired();
		builder.HasOne(e => e.OwnerUser).WithMany(e => e.Connections).HasForeignKey(e => e.OwnerUserId).HasPrincipalKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
	}
}