using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

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

public static class VideoCodecs
{
	public static string GetDefinition(this VideoCodec videoCodec) => videoCodec switch
	{
		VideoCodec.H264 => "libx264",
		VideoCodec.H264Nvidia => "h264_nvenc",
		VideoCodec.H264Intel => "h264_qsv",
		VideoCodec.H264AMD => "h264_amf",
		VideoCodec.WMV2 => "wmv2",
		VideoCodec.VP9 => "libvpx-vp9",
		VideoCodec.VP9Intel => "vp9_qsv",
		VideoCodec.WebP => "libwebp",
		_ => throw new NotImplementedException()
	};

	public static string GetSettings(this VideoCodec videoCodec) => videoCodec switch
	{
		VideoCodec.H264 => string.Empty,
		VideoCodec.H264Nvidia => string.Empty,
		VideoCodec.H264Intel => string.Empty,
		VideoCodec.H264AMD => string.Empty,
		VideoCodec.WMV2 => string.Empty,
		VideoCodec.VP9 => "-cpu-used 5 -b:v 0",
		VideoCodec.VP9Intel => string.Empty,
		VideoCodec.WebP => string.Empty,
		_ => throw new NotImplementedException()
	};

	public static string GetExpression(this VideoCodec videoCodec)
	{
		return $"-c:v {videoCodec.GetDefinition()} {videoCodec.GetSettings()}";
	}

	public static PixelFormat GetDefaultPixelFormat(this VideoCodec videoCodec) => videoCodec switch
	{
		VideoCodec.H264 => PixelFormat.YUV420P,
		VideoCodec.H264Nvidia => PixelFormat.NV12,
		VideoCodec.H264Intel => PixelFormat.NV12,
		VideoCodec.H264AMD => PixelFormat.NV12,
		VideoCodec.WMV2 => PixelFormat.YUV420P,
		VideoCodec.VP9 => PixelFormat.YUV420P,
		VideoCodec.VP9Intel => PixelFormat.NV12,
		VideoCodec.WebP => PixelFormat.YUV420P,
		_ => PixelFormat.None,
	};
}
