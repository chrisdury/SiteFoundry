using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Dury.Imaging
{

	public class ImageResize
	{

		public ImageResize() {}

		public enum Dimensions 
		{
			Width,
			Height
		}
		public enum AnchorPosition
		{
			Top,
			Center,
			Bottom,
			Left,
			Right
		}
		public static Image ScaleByPercent(Image imgPhoto, int Percent)
		{
			float nPercent = ((float)Percent/100);

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;

			int destX = -1;
			int destY = -1; 
			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
			grPhoto.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
			//grPhoto.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			//grPhoto.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
			//grPhoto.Clear(Color.White);

			grPhoto.DrawImage(imgPhoto, 
				new Rectangle(destX,destY,destWidth+1,destHeight+1),
				new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
				GraphicsUnit.Pixel);

			


			grPhoto.Dispose();
			return bmPhoto;

			//return Crop(bmPhoto,destWidth-2,destHeight-2,AnchorPosition.Bottom);


		}

		public static Image ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = -1;
			int destY = -1; 
			float nPercent = 0;

			switch(Dimension)
			{
				case Dimensions.Width:
					nPercent = ((float)Size/(float)sourceWidth);
					break;
				default:
					nPercent = ((float)Size/(float)sourceHeight);
					break;
			}
				
			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
			grPhoto.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
			//grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto, 
				new Rectangle(destX,destY,destWidth+1,destHeight+1),
				new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}


		public static Image FixedSize(Image imgPhoto, int Width, int Height)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = -1;
			int destY = -1; 

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width/(float)sourceWidth);
			nPercentH = ((float)Height/(float)sourceHeight);

			//if we have to pad the height pad both the top and the bottom
			//with the difference between the scaled height and the desired height
			if(nPercentH < nPercentW)
			{
				nPercent = nPercentH;
				destX = (int)((Width - (sourceWidth * nPercent))/2);
			}
			else
			{
				nPercent = nPercentW;
				destY = (int)((Height - (sourceHeight * nPercent))/2);
			}
		
			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.Clear(Color.White);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
			grPhoto.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

			grPhoto.DrawImage(imgPhoto, 
				new Rectangle(destX,destY,destWidth+1,destHeight+1),
				new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}


		public static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width/(float)sourceWidth);
			nPercentH = ((float)Height/(float)sourceHeight);

			if(nPercentH < nPercentW)
			{
				nPercent = nPercentW;
				switch(Anchor)
				{
					case AnchorPosition.Top:
						destY = 0;
						break;
					case AnchorPosition.Bottom:
						destY = (int)(Height - (sourceHeight * nPercent));
						break;
					default:
						destY = (int)((Height - (sourceHeight * nPercent))/2);
						break;
				}				
			}
			else
			{
				nPercent = nPercentH;
				switch(Anchor)
				{
					case AnchorPosition.Left:
						destX = 0;
						break;
					case AnchorPosition.Right:
						destX = (int)(Width - (sourceWidth * nPercent));
						break;
					default:
						destX = (int)((Width - (sourceWidth * nPercent))/2);
						break;
				}			
			}

			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.Clear(Color.White);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto, 
				new Rectangle(destX,destY,destWidth,destHeight),
				new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}

	}


}
