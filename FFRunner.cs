namespace FFCommander;

public class FFRunner
{
	public ProcessRunner ffmpeg;
	public ProcessRunner ffprobe;
	public ProcessRunner ffplay;

	public FFRunner(string executablesPath)
	{
		// Set up FFmpeg
		string ffmpegPath = Path.Combine(executablesPath, "ffmpeg.exe");
		ffmpeg = new(ffmpegPath);

		// Set up FFprobe
		string ffprobePath = Path.Combine(executablesPath, "ffprobe.exe");
		ffprobe = new(ffprobePath);

		// Set up FFplay
		string ffplayPath = Path.Combine(executablesPath, "ffplay.exe");
		ffplay = new(ffplayPath);
	}

	public void Kill()
	{
		ffmpeg.Kill();
		ffprobe.Kill();
		ffplay.Kill();
	}
}
