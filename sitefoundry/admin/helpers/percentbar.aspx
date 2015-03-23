<%@ Page Language="C#" ContentType="text/html" ResponseEncoding="iso-8859-1" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<script language="C#" runat="server">

	void Page_Load(Object sender, EventArgs e)
	{
		Response.Cache.VaryByParams["Used;Width;Height"] = true;
		Response.ContentType = "image/jpeg";
		
		int width = Convert.ToInt32(Request.QueryString["Width"]);
		int height = Convert.ToInt32(Request.QueryString["Height"]);
		int topborder = 0;
		Bitmap objBitmap = new Bitmap(width, height + topborder);
		Graphics objGraphics = Graphics.FromImage(objBitmap);
		int PercentSize = Convert.ToInt32(Request.QueryString["Used"]);
		
		Color PercentBarColor = Color.Green;
		
		if(PercentSize > 80){
			PercentBarColor = Color.Red;
		}
		else{
			PercentBarColor = Color.Green;
		}
				
		float InnerBarWidth = ((float)PercentSize / 100) * (width - 2);
		//Response.Write(InnerBarWidth);
		
		objGraphics.FillRectangle(new SolidBrush(Color.White), 0 , 0, width, height + topborder);
		objGraphics.FillRectangle(new SolidBrush(Color.LightGray), 0, 0 + topborder, width, height);
		objGraphics.FillRectangle(new SolidBrush(Color.White), 1, 1 + topborder, width - 2, height - 2);
		objGraphics.FillRectangle(new SolidBrush(PercentBarColor), 1, 1 + topborder, InnerBarWidth, height - 2);
		objBitmap.Save(Response.OutputStream, ImageFormat.Jpeg);
		objGraphics.Dispose();
		objBitmap.Dispose();
	}
</script>