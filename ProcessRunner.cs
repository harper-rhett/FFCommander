using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

public class ProcessRunner
{
	private string executablePath;
	private Process process;

	public ProcessRunner(string executablePath)
	{
		this.executablePath = $"\"{executablePath}\"";
	}

	public string RunWithOutput(string command, out bool didSucceed)
	{
		// Create process info
		ProcessStartInfo processInfo = new()
		{
			WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
			FileName = executablePath,
			RedirectStandardOutput = true,
			RedirectStandardError = false,
			UseShellExecute = false,
			CreateNoWindow = true,
		};

		// Run command
		Debug.Print($"Running command {processInfo.FileName} {command}");
		processInfo.Arguments = command;
		process = Process.Start(processInfo);

		// Throw exception if process failed to start
		if (process == null) throw new NullReferenceException();

		// Capture output and wait for process to exit
		string output = process.StandardOutput.ReadToEnd();
		process.WaitForExit();

		// Throw an error if something went wrong
		didSucceed = true;
		if (process.ExitCode != 0) didSucceed = false;

		return output;
	}

	public string RunWithError(string command, out bool didSucceed)
	{
		// Create process info
		ProcessStartInfo processInfo = new()
		{
			WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
			FileName = executablePath,
			RedirectStandardOutput = false,
			RedirectStandardError = true,
			UseShellExecute = false,
			CreateNoWindow = true,
		};

		// Run command
		Debug.Print($"Running command {processInfo.FileName} {command}");
		processInfo.Arguments = command;
		process = Process.Start(processInfo);

		// Throw exception if process failed to start
		if (process == null) throw new NullReferenceException();

		// Capture output and wait for process to exit
		string output = process.StandardError.ReadToEnd();
		process.WaitForExit();

		// Throw an error if something went wrong
		didSucceed = true;
		if (process.ExitCode != 0) didSucceed = false;

		return output;
	}

	public void RunWithTerminal(string command, out bool didSucceed)
	{
		// Create process info
		ProcessStartInfo processInfo = new()
		{
			WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
			FileName = executablePath,
			RedirectStandardOutput = false,
			RedirectStandardError = false,
			UseShellExecute = true,
			CreateNoWindow = false,
		};

		// Run command
		Debug.Print($"Running command {processInfo.FileName} {command}");
		processInfo.Arguments = command;
		process = Process.Start(processInfo);

		// Throw exception if process failed to start
		if (process == null) throw new NullReferenceException();

		// Wait for process to exit
		process.WaitForExit();

		// Throw an error if something went wrong
		didSucceed = true;
		if (process.ExitCode != 0) didSucceed = false;
	}

	public void Kill() => process?.Kill();
}
