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

public static class VideoFormats
{
	private static Dictionary<VideoFormat, string> extensions = new()
	{
		{ VideoFormat.MP4, ".mp4" },
		{ VideoFormat.MKV, ".mkv" },
		{ VideoFormat.MOV, ".mov" },
		{ VideoFormat.WMV, ".wmv" },
		{ VideoFormat.AVI, ".avi" },
		{ VideoFormat.GIF, ".gif" },
		{ VideoFormat.WebM, ".webm" },
		{ VideoFormat.WebP, ".webp" },
	};

	private static Dictionary<string, VideoFormat> videoFormats = extensions.ToDictionary(pair => pair.Value, pair => pair.Key);

	public static VideoFormat GetFromExtension(string extension)
	{
		return videoFormats[extension.ToLower()];
	}

	public static string GetExtension(this VideoFormat videoFormat)
	{
		return extensions[videoFormat];
	}

	public static VideoCodec GetDefaultVideoCodec(this VideoFormat videoFormat) => videoFormat switch
	{
		VideoFormat.MP4 => VideoCodec.H264,
		VideoFormat.MKV => VideoCodec.H264,
		VideoFormat.MOV => VideoCodec.H264,
		VideoFormat.WMV => VideoCodec.WMV2,
		VideoFormat.AVI => VideoCodec.H264,
		VideoFormat.GIF => VideoCodec.None,
		VideoFormat.WebM => VideoCodec.VP9,
		VideoFormat.WebP => VideoCodec.WebP,
		_ => throw new NotImplementedException()
	};

	public static AudioCodec GetDefaultAudioCodec(this VideoFormat videoFormat) => videoFormat switch
	{
		VideoFormat.MP4 => AudioCodec.Advanced,
		VideoFormat.MKV => AudioCodec.Advanced,
		VideoFormat.MOV => AudioCodec.Advanced,
		VideoFormat.WMV => AudioCodec.WMAV2,
		VideoFormat.AVI => AudioCodec.MP3,
		VideoFormat.GIF => AudioCodec.None,
		VideoFormat.WebM => AudioCodec.Opus,
		VideoFormat.WebP => AudioCodec.None,
		_ => throw new NotImplementedException()
	};
}