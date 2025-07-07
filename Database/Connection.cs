using Database.Conventions;

namespace Database;

public class Connection : BaseTable
{
	public int OwnerUserId { get; set; }
	public string ApplicationName { get; set; } = default!;
	public string Endpoint { get; set; } = default!;
	public string HeaderSecret { get; set; } = default!;
}
