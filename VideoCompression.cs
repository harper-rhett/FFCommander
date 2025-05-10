using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFCommander;

public class VideoCompression
{
	private double scale;

	public VideoCompression(double scale)
	{
		this.scale = scale;
	}

	public string GetExpression(VideoCodec videoCodec) => videoCodec switch
	{
		VideoCodec.H264 => GetH264Expression(),
		VideoCodec.H264Nvidia => GetH264NvidiaExpression(),
		VideoCodec.H264Intel => GetH264IntelExpression(),
		VideoCodec.H264AMD => GetH264AMDExpression(),
		VideoCodec.WMV2 => GetWMV2Expression(),
		VideoCodec.VP9 => GetVP9Expression(),
		VideoCodec.VP9Intel => GetVP9IntelExpression(),
		VideoCodec.WebP => GetWebPExpression(),
		_ => throw new NotImplementedException()
	};

	private static double Lerp(double start, double end, double value)
	{
		return start + (end - start) * value;
	}

	public string GetH264Expression()
	{
		int compression = (int)Lerp(18, 51, scale);
		return $"-preset medium -crf {compression}";
	}

	public string GetH264NvidiaExpression()
	{
		int compression = (int)Lerp(19, 51, scale);
		return $"-preset p4 -rc vbr -cq {compression} -b:v 0";
	}

	public string GetH264IntelExpression()
	{
		int compression = (int)Lerp(16, 51, scale);
		return $"-preset medium -global_quality {compression} -rc vbr";
	}

	public string GetH264AMDExpression()
	{
		int compression = (int)Lerp(15, 51, scale);
		return $"-rc vbr_quality -quality quality -qp {compression}";
	}

	public string GetWMV2Expression()
	{
		int bitrate = (int)Lerp(2500, 500, scale); // in kbps
		return $"-b:v {bitrate}k";
	}

	public string GetVP9Expression()
	{
		int compression = (int)Lerp(15, 50, scale);
		return $"-crf {compression}";
	}

	public string GetVP9IntelExpression()
	{
		int compression = (int)Lerp(16, 51, scale);
		return $"-global_quality:v {compression} -rc:v vbr";
	}

	public string GetWebPExpression()
	{
		int compression = (int)Lerp(100, 0, scale);
		return $"-quality {compression}";
	}
}