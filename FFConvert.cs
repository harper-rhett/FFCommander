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

	public class Options
	{
		public string InputMediaPath;
		public VideoFormat VideoFormat;
		public VideoCodec VideoCodec;
		public PixelFormat PixelFormat;
		public AudioCodec AudioCodec;
		public Filters Filters;
		public string CustomExpressions;

		// Things to add:
		// Strip audio
		// Loop video

		public Options(
			string inputMediaPath,
			VideoFormat videoFormat = VideoFormat.None,
			VideoCodec videoCodec = VideoCodec.None,
			PixelFormat pixelFormat = PixelFormat.None,
			AudioCodec audioCodec = AudioCodec.None,
			Filters filters = null,
			string customExpressions = ""
		)
		{
			// Set paths
			InputMediaPath = inputMediaPath;

			// Check input
			bool hasVideoFormat = videoFormat != VideoFormat.None;
			bool hasVideoCodec = videoCodec != VideoCodec.None;
			bool hasPixelFormat = pixelFormat != PixelFormat.None;
			bool hasAudioCodec = audioCodec != AudioCodec.None;
			bool hasFilters = filters != null;

			// Set defaults
			VideoFormat = hasVideoFormat ? videoFormat : ExtractVideoFormat(inputMediaPath);
			VideoCodec = hasVideoCodec ? videoCodec : VideoFormat.GetDefaultVideoCodec();
			PixelFormat = hasPixelFormat ? pixelFormat : VideoCodec.GetDefaultPixelFormat();
			AudioCodec = hasAudioCodec ? audioCodec : VideoFormat.GetDefaultAudioCodec();
			Filters = hasFilters ? filters : new();
		}

		private VideoFormat ExtractVideoFormat(string inputMediaPath)
		{
			string extension = Path.GetExtension(inputMediaPath);
			return VideoFormats.GetFromExtension(extension);
		}
	}

	public void Run(Options options, string outputFolderPath, string outputMediaName, out string command, out bool didSucceed, out string outputMediaPath)
	{
		// Check again for values
		bool hasVideoCodec = options.VideoCodec != VideoCodec.None;
		bool hasPixelFormat = options.PixelFormat != PixelFormat.None;
		bool hasAudioCodec = options.AudioCodec != AudioCodec.None;

		// Build command
		const string replaceExpression = $"-y";
		string inputExpression = $"-i \"{options.InputMediaPath}\"";
		string codecExpression = hasVideoCodec ? $"-c:v {options.VideoCodec.GetDefinition()}" : string.Empty;
		string pixelFormatExpression = hasPixelFormat ? $"-pix_fmt {options.PixelFormat.GetDefinition()}" : string.Empty;
		string audioCodecExpression = hasAudioCodec ? $"-c:a {options.AudioCodec.GetDefinition()}" : string.Empty;
		string filtersExpression = options.Filters.GetFinalExpression();
		outputMediaPath = Path.ChangeExtension(Path.Combine(outputFolderPath, outputMediaName), options.VideoFormat.GetExtension());
		string outputExpression = $"\"{outputMediaPath}\"";

		// Run command
		command = $"{replaceExpression} {inputExpression} {codecExpression} {pixelFormatExpression} {audioCodecExpression} {filtersExpression} {outputExpression}";
		command = Regex.Replace(command, @"\s{2,}", " ");
		RunWithTerminal(command, out didSucceed);
	}

	public bool DoesVideoCodecRun(VideoCodec videoCodec)
	{
		// Test codec
		string codecDefinition = videoCodec.GetDefinition();
		string output = RunWithError($"-f lavfi -i testsrc -t 0.25 -c:v {codecDefinition} -f null -", out bool didSucceed);
		return didSucceed;
	}
}