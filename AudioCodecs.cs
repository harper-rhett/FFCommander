using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

public enum AudioCodec
{
	None,
	Advanced,
	Opus,
	WMAV2,
	MP3
}

public static class AudioCodecs
{
	public static string GetDefinition(this AudioCodec audioCodec) => audioCodec switch
	{
		AudioCodec.Advanced => "aac",
		AudioCodec.Opus => "libopus",
		AudioCodec.WMAV2 => "wmav2",
		AudioCodec.MP3 => "mp3",
		_ => throw new NotImplementedException()
	};

	public static string GetExpression(this AudioCodec audioCodec)
	{
		return $"-c:a {audioCodec.GetDefinition()}";
	}
}
