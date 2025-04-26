using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FFCommander;

public class FFConvert : ProcessRunner
{
	public FFConvert(string executablePath) : base(executablePath) { }

	public void Run(
		string inputMediaPath, string outputFolderPath, string outputMediaName,
		VideoFormat videoFormat = VideoFormat.None,
		VideoCodec videoCodec = VideoCodec.None,
		PixelFormat pixelFormat = PixelFormat.None,
		AudioCodec audioCodec = AudioCodec.None,
		Filters filters = null,
		string customExpressions = ""

		// Things to add:
		// Strip audio
		// Loop video
	)
	{
		// Check for inputs
		bool hasVideoFormat = videoFormat != VideoFormat.None;
		bool hasVideoCodec = videoCodec != VideoCodec.None;
		bool hasPixelFormat = pixelFormat != PixelFormat.None;
		bool hasAudioCodec = audioCodec != AudioCodec.None;
		bool hasFilters = filters != null;

		// Set defaults
		if (!hasVideoFormat) videoFormat = ExtractVideoFormat(inputMediaPath);
		if (!hasVideoCodec) videoCodec = videoFormat.GetDefaultVideoCodec();
		if (!hasPixelFormat) pixelFormat = videoCodec.GetDefaultPixelFormat();
		if (!hasAudioCodec) audioCodec = videoFormat.GetDefaultAudioCodec();
		if (!hasFilters) filters = new();

		// Check again for values
		hasVideoCodec = videoCodec != VideoCodec.None;
		hasPixelFormat = pixelFormat != PixelFormat.None;
		hasAudioCodec = audioCodec != AudioCodec.None;

		// Build command
		const string replaceExpression = $"-y";
		string inputExpression = $"-i {inputMediaPath}";
		string codecExpression = hasVideoCodec ? $"-c:v {videoCodec.GetDefinition()}" : string.Empty;
		string pixelFormatExpression = hasPixelFormat ? $"-pix_fmt {pixelFormat.GetDefinition()}" : string.Empty;
		string audioCodecExpression = hasAudioCodec ? $"-c:a {audioCodec.GetDefinition()}" : string.Empty;
		string filtersExpression = filters.GetFinalExpression();
		string outputExpression = Path.Combine(outputFolderPath, outputMediaName, videoFormat.GetExtension());

		// Run command
		string command = $"{replaceExpression} {inputExpression} {codecExpression} {pixelFormatExpression} {audioCodecExpression} {filtersExpression} {outputExpression}";
		command = Regex.Replace(command, @"\s{2,}", " ");
		RunWithTerminal(command, out bool didSucceed);
	}

	private VideoFormat ExtractVideoFormat(string inputMediaPath)
	{
		string extension = Path.GetExtension(inputMediaPath);
		return VideoFormats.GetFromExtension(extension);
	}

	public bool DoesVideoCodecRun(VideoCodec videoCodec)
	{
		// Test codec
		string codecDefinition = videoCodec.GetDefinition();
		string output = RunWithError($"-f lavfi -i testsrc -t 0.25 -c:v {codecDefinition} -f null -", out bool didSucceed);
		if (!didSucceed) throw new Exception("Codec test failed.");

		// Test codec output
		bool doesRun = !output.Contains("error while opening encoder", StringComparison.OrdinalIgnoreCase);
		return doesRun;
	}
}