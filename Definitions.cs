using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

public enum VideoFormat
{
	None,
	MP4,
	MKV,
	MOV,
	WMV,
	AVI,
	GIF,
	WebM,
	WebP
}

public enum VideoCodec
{
	None,
	H264,
	H264Nvidia,
	H264Intel,
	H264AMD,
	WMV2,
	VP9,
	VP9Intel,
	WebP,
}

public enum PixelFormat
{
	None,
	YUV420P,
	NV12
}

internal static class Definitions
{
	public static string ToExtension(this VideoFormat videoFormat) => videoFormat switch
	{
		VideoFormat.MP4 => ".mp4",
		VideoFormat.MKV => ".mkv",
		VideoFormat.MOV => ".mov",
		VideoFormat.WMV => ".wmv",
		VideoFormat.AVI => ".avi",
		VideoFormat.GIF => ".gif",
		VideoFormat.WebM => ".webm",
		VideoFormat.WebP => ".webp",
		_ => throw new NotImplementedException()
	};

	public static string ToDefinition(this PixelFormat pixelFormat) => pixelFormat switch
	{
		PixelFormat.YUV420P => "yuv420p",
		PixelFormat.NV12 => "nv12",
		_ => throw new NotImplementedException()
	};

	public static string ToDefinition(this VideoCodec videoCodec) => videoCodec switch
	{
		VideoCodec.H264 => "libx264",
		_ => throw new NotImplementedException()
	};
}
