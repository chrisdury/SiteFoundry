using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Dury.SiteFoundry;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Summary description for genImage.
	/// </summary>
	public class GenImage : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (Request.QueryString["f"] == null) return;

			string filename = SFGlobal.BaseDirectory + SFGlobal.ResourceFileLocation + "/" + Request.QueryString["f"];
		
			int w = (Request.QueryString["w"] != null) ? int.Parse(Request.QueryString["w"]) : 0;
			int h = (Request.QueryString["h"] != null) ? int.Parse(Request.QueryString["h"]) : 0;

			System.Drawing.Image img = System.Drawing.Image.FromFile(filename);

			//guess height and width if only 1 specified
			if (h==0 && w==0)
			{
				h = img.Height;
				w = img.Width;
			} 
			else 
			{
				if (h==0)
				{
					h = (int)Math.Ceiling((double)img.Height / ((float) img.Width / (float) w));
				}
				if (w==0)
				{
					w = (int)Math.Ceiling((double)img.Width / ((float) img.Height / (float) h));
				}
			}

			Rectangle rect = new Rectangle(0, 0, w, h);
			float f1 = (float) img.Width / (float) w; 
			float f2 = (float) img.Height / (float) h; 
			float f = Math.Max (f1, f2); 
			rect.Width = (int) (img.Width / f); 
			rect.Height = (int) (img.Height / f); 
			rect.X = (w - rect.Width) / 2; 
			rect.Y = (h - rect.Height) / 2;


			// get our objects to work with
			System.Drawing.Image thumb = new Bitmap(w,h,img.PixelFormat);
			Graphics g =  Graphics.FromImage(thumb);
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
		
			
			// fill w/ background color if specified
			if (Request.QueryString["c"] != null) 
			{
				string htmlColor = "#" + Request.QueryString["c"];
				System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(htmlColor);
				g.FillRectangle(new System.Drawing.SolidBrush(c),-1,-1,w+1,h+1);
			}

		
			// maintain aspect ratio
			/*
			Encoder encoderInstance=Encoder.Quality;
			EncoderParameters[] encoderParametersInstance=new EncoderParameters(2);
			EncoderParameter encoderParameterInstance=new EncoderParameter(encoderInstance, 80L);
			encoderParametersInstance.Param[0]=encoderParameterInstance;
			encoderInstance=Encoder.ColorDepth;
			encoderParameterInstance=new EncoderParameter(encoderInstance, 24L);
			encoderParametersInstance.Param[1]=encoderParameterInstance;
			*/

			// write out image, then close objects
			g.DrawImage(img,rect,0,0,img.Width,img.Height,GraphicsUnit.Pixel);
			MemoryStream ms = new MemoryStream();
			thumb.Save(ms,System.Drawing.Imaging.ImageFormat.Jpeg);
			Response.BinaryWrite(ms.ToArray());
			g.Dispose();
			img.Dispose();
			thumb.Dispose();
			ms.Close();
			Response.End();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
