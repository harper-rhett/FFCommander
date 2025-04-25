using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

public class FFConvert : ProcessRunner
{
	public FFConvert(string executablePath) : base(executablePath) { }

	public void Run(
		string inputMediaPath, string outputFolderPath, string outputMediaName,
		VideoFormat videoFormat = VideoFormat.None,
		VideoCodec videoCodec = VideoCodec.None,
		PixelFormat pixelFormat = PixelFormat.None
	)
	{
		// Set defaults
		if (videoFormat == VideoFormat.None) videoFormat = ExtractVideoFormat(inputMediaPath);
		if (videoCodec == VideoCodec.None) videoCodec = videoFormat.GetDefaultCodec();
		if (pixelFormat == PixelFormat.None) pixelFormat = videoCodec.GetDefaultPixelFormat();
	}

	private VideoFormat ExtractVideoFormat(string inputMediaPath)
	{
		string extension = Path.GetExtension(inputMediaPath);
		return Definitions.GetVideoFormat(extension);
	}
}
