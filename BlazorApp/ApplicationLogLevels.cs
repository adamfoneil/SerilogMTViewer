using Serilog.Core;
using Serilog.Events;
using SerilogBlazor.Abstractions;

namespace BlazorApp;

public class ApplicationLogLevels() : LogLevels(LogEventLevel.Debug)
{
	private readonly Dictionary<string, LoggingLevelSwitch> _levels = new()
	{
		["Microsoft"] = new(LogEventLevel.Warning),
		["System"] = new(LogEventLevel.Warning)
	};

	public override Dictionary<string, LoggingLevelSwitch> LoggingLevels => throw new NotImplementedException();
}
