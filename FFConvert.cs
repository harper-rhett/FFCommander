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
		public AudioChannels audioChannels;
		public Loop Loop;
		public Filters Filters;

		public Options(
			string inputMediaPath,
			VideoFormat videoFormat = VideoFormat.None,
			VideoCodec videoCodec = VideoCodec.None,
			PixelFormat pixelFormat = PixelFormat.None,
			AudioCodec audioCodec = AudioCodec.None,
			AudioChannels audioChannels = null,
			Loop loop = Loop.Ignore,
			Filters filters = null
		)
		{
			// Set paths
			InputMediaPath = inputMediaPath;

			// Check input
			bool hasVideoFormat = videoFormat != VideoFormat.None;
			bool hasVideoCodec = videoCodec != VideoCodec.None;
			bool hasPixelFormat = pixelFormat != PixelFormat.None;
			bool hasAudioCodec = audioCodec != AudioCodec.None;
			bool hasAudioChannels = audioChannels != null;
			bool hasFilters = filters != null;

			// Set defaults
			VideoFormat = hasVideoFormat ? videoFormat : ExtractVideoFormat(inputMediaPath);
			VideoCodec = hasVideoCodec ? videoCodec : VideoFormat.GetDefaultVideoCodec();
			PixelFormat = hasPixelFormat ? pixelFormat : VideoCodec.GetDefaultPixelFormat();
			AudioCodec = hasAudioCodec ? audioCodec : VideoFormat.GetDefaultAudioCodec();
			audioChannels = hasAudioChannels ? audioChannels : new(2);
			Filters = hasFilters ? filters : new();
		}

		private VideoFormat ExtractVideoFormat(string inputMediaPath)
		{
			string extension = Path.GetExtension(inputMediaPath);
			return VideoFormats.GetFromExtension(extension);
		}

		public string InputExpression
		{
			get { return $"-y -i \"{InputMediaPath}\""; }
		}
	}

	public void Run(Options options, string outputFolderPath, string outputMediaName, out string command, out bool didSucceed, out string outputMediaPath)
	{
		// Check again for values
		bool hasVideoCodec = options.VideoCodec != VideoCodec.None;
		bool hasPixelFormat = options.PixelFormat != PixelFormat.None;
		bool hasAudioCodec = options.AudioCodec != AudioCodec.None;

		// Build base command
		List<string> expressions = new();
		expressions.Add(options.InputExpression);
		if (hasVideoCodec) expressions.Add(options.VideoCodec.GetExpression());
		if (hasPixelFormat) expressions.Add(options.PixelFormat.GetExpression());
		if (hasAudioCodec) expressions.Add(options.AudioCodec.GetExpression());
		expressions.Add(options.Filters.FinalExpression);
		expressions.Add(options.audioChannels.Expression);
		expressions.Add(options.Loop.GetExpression());

		// Build output path expression
		outputMediaPath = Path.ChangeExtension(Path.Combine(outputFolderPath, outputMediaName), options.VideoFormat.GetExtension());
		expressions.Add($"\"{outputMediaPath}\""); // output

		// Run command
		command = string.Join(" ", expressions);
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