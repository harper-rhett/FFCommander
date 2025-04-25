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
		VideoFormat outputVideoFormat = VideoFormat.None,
		VideoCodec videoCodec = VideoCodec.None,
		PixelFormat pielFormat = PixelFormat.None
	)
	{
		// Set defaults
		string outputExtension = GetOutputExtension(outputVideoFormat, inputMediaPath);
	}

	private string GetOutputExtension(VideoFormat videoFormat, string inputMediaPath)
	{
		bool hasVideoFormat = videoFormat != VideoFormat.None;
		string extension = hasVideoFormat ? videoFormat.ToExtension() : Path.GetExtension(inputMediaPath);
		return extension;
	}
}
