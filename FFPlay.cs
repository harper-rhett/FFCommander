using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

public class FFPlay : ProcessRunner
{
	public FFPlay(string executablePath) : base(executablePath) { }

	public void Video(string videoPath, string windowTitle = "FFPlay", bool isMuted = false)
	{
		string muteExpression = isMuted ? "-an" : string.Empty;
		string titleExpression = $"-window_title \"{windowTitle}\"";
		string sanitizedVideoPath = $"\"{videoPath}\"";
		string command = $"{muteExpression} {windowTitle} {sanitizedVideoPath}";

		RunWithOutput(command, out bool didSucceed);
		if (!didSucceed) throw new Exception("FFplay playback failed.");
	}
}
