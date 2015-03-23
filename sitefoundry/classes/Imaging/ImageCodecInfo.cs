using System;
using System.Drawing.Imaging;

namespace Dury.Imaging
{
	/// <summary>
	/// Summary description for ImageCodecInfo.
	/// </summary>
	public class Encoders
	{
		public Encoders() {}


		public static ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for(int j=0; j<encoders.Length; j++)
			{
				if(encoders[j].MimeType == mimeType) return encoders[j];
			} 
			return null;
		}
	}
}
