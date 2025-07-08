using AuthExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Claims;

namespace Database;

public class ApplicationUser : IdentityUser, IClaimData
{
	public int UserId { get; set; }
	public string? TimeZoneId { get; set; }

	public ICollection<Connection> Connections { get; set; } = [];

	public void FromClaims(IEnumerable<Claim> claims)
	{
		UserId = int.Parse(claims.GetClaimValue(nameof(UserId)) ?? "0");
		TimeZoneId = claims.GetClaimValue("TimeZoneId");
		PhoneNumber = claims.GetClaimValue(ClaimTypes.MobilePhone);
	}

	public IEnumerable<Claim> ToClaims()
	{
		yield return new Claim(nameof(UserId), UserId.ToString());

		if (!string.IsNullOrEmpty(TimeZoneId))
		{
			yield return new Claim("TimeZoneId", TimeZoneId);
		}

		if (!string.IsNullOrEmpty(PhoneNumber))
		{
			yield return new Claim(ClaimTypes.MobilePhone, PhoneNumber);
		}
	}
}

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{
		builder.Property(e => e.UserId).ValueGeneratedOnAdd();
		builder.HasIndex(e => e.UserId).IsUnique();
		builder.Property(u => u.TimeZoneId).HasMaxLength(50);
	}
}
