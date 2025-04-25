namespace FFCommander;

public class FFRunner
{
	public FFMPEG MPEG;
	public FFProbe Probe;
	public FFPlay Play;

	public FFRunner(string executablesPath)
	{
		// Set up FFmpeg
		string ffmpegPath = Path.Combine(executablesPath, "ffmpeg.exe");
		MPEG = new(ffmpegPath);

		// Set up FFprobe
		string ffprobePath = Path.Combine(executablesPath, "ffprobe.exe");
		Probe = new(ffprobePath);

		// Set up FFplay
		string ffplayPath = Path.Combine(executablesPath, "ffplay.exe");
		Play = new(ffplayPath);
	}

	public void Kill()
	{
		MPEG.Kill();
		Probe.Kill();
		Play.Kill();
	}
}
