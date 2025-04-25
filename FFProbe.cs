using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

public class FFProbe : ProcessRunner
{
	public FFProbe(string executablePath) : base(executablePath) { }

	public int Width(string videoPath)
	{
		string output = RunWithOutput($"-v error -select_streams \"v:0\" -show_entries \"stream=width\" -of \"csv=s=x:p=0\" \"{videoPath}\"", out bool didSucceed);
		if (!didSucceed) throw new Exception("Width probe failed.");
		int width = int.Parse(output);
		return width;
	}

	public int Height(string videoPath)
	{
		string output = RunWithOutput($"-v error -select_streams \"v:0\" -show_entries \"stream=height\" -of \"csv=s=x:p=0\" \"{videoPath}\"", out bool didSucceed);
		if (!didSucceed) throw new Exception("Height probe failed.");
		int height = int.Parse(output);
		return height;
	}

	public double FrameRate(string videoPath)
	{
		string output = RunWithOutput($"-v error -select_streams v -of default=noprint_wrappers=1:nokey=1 -show_entries stream=r_frame_rate \"{videoPath}\"", out bool didSucceed);
		if (!didSucceed) throw new Exception("Frame rate probe failed.");
		string[] parts = output.Split("/");
		double frameRate = double.Parse(parts[0]) / double.Parse(parts[1]);
		return frameRate;
	}
}
