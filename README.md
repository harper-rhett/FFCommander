FFCommander is a tool which interfaces with FFmpeg, FFprobe, and FFplay through its command line.

It can be used for sending direct commands:
```csharp
FFRunner.Convert.RunWithTerminal("ffmpeg -i input.mp4 -vf scale=640:360 output.mp4", out bool didSucceed);
```

Or with some limited pre-built functionality:
```csharp
FFConvert.Options options = new(
	inputVideoPath,
	VideoFormat.MOV, VideoCodec.H264,
	videoCompression: new(0.5)
);
FFRunner.Convert.Run(
  options, outputFolderPath, outputVideoName,
  out string command, out bool didSucceed, out string outputVideoPath
);
```
```csharp
int width = FFRunner.Probe.Width(videoPath);
```
```csharp
FFRunner.Play.Video(videoPath, windowTitle: "My Video", isMuted: true);
```

And is super easy to set up:
```csharp
using FFCommander;

string currentDirectory = Directory.GetCurrentDirectory();
string ffPath = Path.Combine(currentDirectory, "Executables");
FFRunner ffRunner = new(ffPath);
```

Though it is missing lots of features!
It was primarily created for my own purposes in supporting [Convertophile](https://harper-rhett.itch.io/convertophile).
So, I add features when I need them.
If you'd like to use it yourself beyond the features I have implemented, give it a fork! It's relatively plug and play.
