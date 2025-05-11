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
		public VideoCompression VideoCompression;
		public AudioChannels AudioChannels;
		public Loop Loop;
		public Filters Filters;

		public Options(
			string inputMediaPath,
			VideoFormat videoFormat = VideoFormat.None,
			VideoCodec videoCodec = VideoCodec.None,
			PixelFormat pixelFormat = PixelFormat.None,
			AudioCodec audioCodec = AudioCodec.None,
			VideoCompression videoCompression = null,
			AudioChannels audioChannels = null,
			Loop loop = Loop.Ignore,
			Filters filters = null
		)
		{
			InputMediaPath = inputMediaPath;
			VideoFormat = videoFormat;
			VideoCodec = videoCodec;
			PixelFormat = pixelFormat;
			AudioCodec = audioCodec;
			VideoCompression = videoCompression;
			AudioChannels = audioChannels;
			Loop = loop;
			if (filters == null) Filters = new();
		}

		public void InitializeEmpty()
		{
			// Check settings
			bool hasVideoFormat = VideoFormat != VideoFormat.None;
			bool hasVideoCodec = VideoCodec != VideoCodec.None;
			bool hasPixelFormat = PixelFormat != PixelFormat.None;
			bool hasAudioCodec = AudioCodec != AudioCodec.None;
			bool hasAudioChannels = AudioChannels != null;

			// Set defaults
			if (!hasVideoFormat) VideoFormat = ExtractVideoFormat(InputMediaPath);
			if (!hasVideoCodec) VideoCodec = VideoFormat.GetDefaultVideoCodec();
			if (!hasPixelFormat) PixelFormat = VideoCodec.GetDefaultPixelFormat();
			if (!hasAudioCodec) AudioCodec = VideoFormat.GetDefaultAudioCodec();
			if (!hasAudioChannels) AudioChannels = new(2);
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
		options.InitializeEmpty();
		bool hasVideoCodec = options.VideoCodec != VideoCodec.None;
		bool hasPixelFormat = options.PixelFormat != PixelFormat.None;
		bool hasAudioCodec = options.AudioCodec != AudioCodec.None;
		bool hasVideoCompression = options.VideoCompression != null;

		// Build base command
		List<string> expressions = new();
		expressions.Add(options.InputExpression);
		if (hasVideoCodec) expressions.Add(options.VideoCodec.GetExpression());
		if (hasVideoCompression) expressions.Add(options.VideoCompression.GetExpression(options.VideoCodec));
		if (hasPixelFormat) expressions.Add(options.PixelFormat.GetExpression());
		if (hasAudioCodec) expressions.Add(options.AudioCodec.GetExpression());
		expressions.Add(options.AudioChannels.Expression);
		expressions.Add(options.Filters.FinalExpression);
		expressions.Add(options.Loop.GetExpression());

		// Build output path expression
		outputMediaPath = Path.ChangeExtension(Path.Combine(outputFolderPath, outputMediaName), options.VideoFormat.GetExtension());
		expressions.Add($"\"{outputMediaPath}\"");

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