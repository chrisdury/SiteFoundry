using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Web;

using Dury.SiteFoundry;


namespace Dury.SiteFoundry.HTTPHandlers
{
    
	/// <summary>
	/// HTTP Handler for retrieving dynamic images.
	/// </summary>
	public class ImageHandler : IHttpHandler 
	{        
		public bool IsReusable 
		{
			get { return true; }
		}


		private int maxGeneratedImages = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["img_handler_generated_count"]);
        private string handlerFileName = System.Configuration.ConfigurationSettings.AppSettings["img_handler_name"];
		private ImageHandlerSource imgSource = (ImageHandlerSource)Enum.Parse(typeof(ImageHandlerSource),System.Configuration.ConfigurationSettings.AppSettings["img_handler_source"],true);
		private string imgGeneratedDirectory = System.Configuration.ConfigurationSettings.AppSettings["img_handler_generated_directory"];
		private int imgGeneratedQuality = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["img_handler_generated_quality"]);
		private ImageInfo currentImage;
		private string resourceDirectory = SFGlobal.SiteRoot + SFGlobal.ResourceFileLocation + "/";


		System.Web.HttpContext context;
		public void ProcessRequest(System.Web.HttpContext c) 
		{
			context = c;
			string r = context.Request.RawUrl;

			resourceDirectory = c.Server.MapPath(resourceDirectory);

			Random random = new Random();
			int i = random.Next(maxGeneratedImages);
			if (i == (maxGeneratedImages-50)) cleanUpGeneratedImages();

			// lets get our url and make an image struct for us to work on.
			string filename = context.Request.QueryString["file"];//r.Substring(r.IndexOf(handlerFileName) + handlerFileName.Length + 1);
			int width = int.Parse(context.Request.QueryString["w"]);
			int height = int.Parse(context.Request.QueryString["h"]);


			currentImage = new ImageInfo(filename, width, height, imgGeneratedQuality);

			// lets stop people from making huge images that would eat up hd space
			if (currentImage.Height > 2000 || currentImage.Width > 2000) error();

			// we got the image now.. so lets check to see if the source exists
			if (!checkImageExists(currentImage,true)) error();

			// lets see if the generated version exists. if not we're gonna make it
			if (!checkImageExists(currentImage,false)) generateImage(currentImage);

			// transmit image to client.
			c.Response.Cache.SetLastModified(DateTime.Now.AddYears(-1));
			c.Response.WriteFile(this.imgGeneratedDirectory + currentImage.SFName);
			//context.Response.TransmitFile(this.resourceDirectory + this.imgGeneratedDirectory + currentImage.SFName);
		}

		private void cleanUpGeneratedImages() 
		{
			//int maxGeneratedImages = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["img_handler_generated_count"]);
			// check image count threshold
			string[] files = System.IO.Directory.GetFiles(context.Server.MapPath(this.imgGeneratedDirectory));
			if (files.Length < maxGeneratedImages) return;

			System.Data.DataTable dt = new System.Data.DataTable();
			dt.Columns.Add("filename",typeof(string));
			dt.Columns.Add("lastaccessed",typeof(DateTime));
			
			foreach(string f in files) 
			{
				System.IO.FileInfo fi = new System.IO.FileInfo(f);
				dt.Rows.Add(new object[] { fi.FullName, fi.LastAccessTime } );
			}
			dt.DefaultView.Sort = "lastaccessed ASC";
			int maxImages = (int)Math.Floor((double)maxGeneratedImages / (double)5);

			for (int i=0; i<dt.DefaultView.Count; i++)
			{
				if (i >= maxImages) return;
				System.IO.File.Delete(dt.DefaultView[i]["filename"].ToString());
			}			
		}

		private void error() 
		{
			context.Response.Write("there's been a problem, so sorry :-( ");
			context.Response.End();
		}

		private void generateImage(ImageInfo i) 
		{
			Image img = System.Drawing.Image.FromFile(this.resourceDirectory + i.Filename);
			Image thumb;
			/*
			if (i.Width>i.Height)
				thumb = Dury.Imaging.ImageResize.ConstrainProportions(img,i.Width,Dury.Imaging.ImageResize.Dimensions.Width);
			else
				thumb = Dury.Imaging.ImageResize.ConstrainProportions(img,i.Height,Dury.Imaging.ImageResize.Dimensions.Height);
			*/
			thumb = Dury.Imaging.ImageResize.FixedSize(img,i.Width,i.Height);
			Encoder qualityEncoder = Encoder.Quality;
			EncoderParameter ratio = new EncoderParameter(qualityEncoder, (long)i.Quality);
			EncoderParameters codecParams = new EncoderParameters(1);
			codecParams.Param[0] = ratio;
			ImageCodecInfo CodecInfo=Dury.Imaging.Encoders.GetEncoderInfo("image/jpeg");
			thumb.Save(context.Server.MapPath(this.imgGeneratedDirectory) + i.SFName,CodecInfo,codecParams);
			thumb.Dispose();
			img.Dispose();
			ratio.Dispose();
			codecParams.Dispose();
		}

		private bool checkImageExists(ImageInfo i, bool originalImage) 
		{
			string f = "";
			string path = context.Server.MapPath(SFGlobal.SiteRoot + SFGlobal.ResourceFileLocation);
			if (!originalImage) path += "/" + imgGeneratedDirectory;
			f = (originalImage) ? i.Filename : i.SFName;
			return System.IO.File.Exists(path + "/" + f);
			//return false;
		}

	}

	public enum ImageHandlerSource
	{
		FileSystem=0,
		SqlServer
	}

	public struct ImageInfo
	{
		public string Filename;
		public int Width;
		public int Height;
		public int Quality;

		public ImageInfo(string f, int w, int h, int q)
		{
			Filename = f;
			Width = w;
			Height = h;
			Quality = q;
		}

		public string SFName
		{
			get 
			{
				return Filename.Insert(Filename.LastIndexOf('.'),"_" + Width + "x" + Height).Replace("/",";;");
			}
		}


		public override string ToString() 
		{
			return "image: f=" + this.Filename + ", w=" + this.Width + ", h=" + this.Height + ", q=" + this.Quality;
		}
	}

}
