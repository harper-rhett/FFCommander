using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

public enum PixelFormat
{
	None,
	YUV420P,
	NV12
}

public static class PixelFormats
{
	public static string GetDefinition(this PixelFormat pixelFormat) => pixelFormat switch
	{
		PixelFormat.YUV420P => "yuv420p",
		PixelFormat.NV12 => "nv12",
		_ => throw new NotImplementedException()
	};
}
