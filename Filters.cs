using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

public class Filters
{
	private List<string> expressions = new();

	public void AddExpression(string expression) => expressions.Add(expression);

	public void AddScale(int width, int height, FilterFlags.Resample resample = FilterFlags.Resample.Bilinear)
	{
		AddExpression($"scale={width}:{height}:{resample.GetDefinition()}");
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
		AddExpression($"split[a][b];[a]palettegen=max_colors={maxColors}[p];[b][p]paletteuse");
	}

	public void Reset() => expressions.Clear();

	public string GetFinalExpression()
	{
		return $"-vf \"{string.Join(",", expressions)}\"";
	}
}

public static class FilterFlags
{
	public enum Resample
	{
		Bilinear,
		FastBilinear,
		Bicubic,
		Lanczos,
		Spline,
		Point
	}

	public static string GetDefinition(this Resample resampling) => resampling switch
	{
		Resample.Bilinear => "bilinear",
		Resample.FastBilinear => "fast_bilinear",
		Resample.Bicubic => "bicubic",
		Resample.Lanczos => "lanczos",
		Resample.Spline => "spline",
		Resample.Point => "point",
		_ => throw new NotImplementedException()
	};
}