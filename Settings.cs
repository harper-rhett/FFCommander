using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

public enum Loop
{
    Ignore,
    Once,
    Infinite
}

public static class Settings
{
	public static string GetExpression(this Loop loop) => loop switch
	{
		Loop.Ignore => string.Empty,
		Loop.Once => "-loop -1",
		Loop.Infinite => "-loop 0",
		_ => string.Empty
	};
}
