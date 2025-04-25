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
		PixelFormat pixelFormat = PixelFormat.None,
		AudioCodec audioCodec = AudioCodec.None,
		bool stripAudio = false,
		Filters filters = null,
		string customExpressions = ""
	)
	{
		// Set defaults
		if (videoFormat == VideoFormat.None) videoFormat = ExtractVideoFormat(inputMediaPath);
		if (videoCodec == VideoCodec.None) videoCodec = videoFormat.GetDefaultVideoCodec();
		if (pixelFormat == PixelFormat.None) pixelFormat = videoCodec.GetDefaultPixelFormat();
		if (audioCodec == AudioCodec.None) audioCodec = videoFormat.GetDefaultAudioCodec();

		// up next:
		// consider a universal codec compression system
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

public class Filters
{
	private List<string> expressions = new();

	public void AddExpression(string expression) => expressions.Add(expression);

	// Need to add flags!
	// Gif uses lanczos
	// Don't need to specify for other file types

	public void AddScale(int width, int height)
	{
		AddExpression($"scale={width}:{height}");
	}

	public void AddFrameRate(double frameRate)
	{
		AddExpression($"fps={frameRate}");
	}

	public void AddSubtitles(string subtitlesPath)
	{
		string sanitizedPath = @$"{subtitlesPath.Replace("\\", "/").Replace(":", "\\:")}";
		AddExpression($"subtitles={sanitizedPath}");
	}

	public void AddPaletteLimit(int maxColors)
	{
		//split[a][b];[a]palettegen=max_colors=[max colors][p];[b][p]paletteuse
	}

	public string GetFinalExpression()
	{
		return $"-vf \"{string.Join(",", expressions)}\"";
	}
}