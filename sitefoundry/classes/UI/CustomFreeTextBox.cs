using System;
using FreeTextBoxControls;

namespace Dury.SiteFoundry.UI
{
	public class CustomFreeTextBox : FreeTextBox
	{

		public CustomFreeTextBox()
		{
			//if (this.ToolbarLayout == null || this.ToolbarLayout == "" )
			//{
				this.ToolbarLayout = System.Configuration.ConfigurationSettings.AppSettings["tb_toolbar"];
			//}

			FreeTextBoxControls.Toolbar tb = new Toolbar();


			ToolbarButton addImageButton = new ToolbarButton("Insert an Image","FTB_OpenImageWindow","insertimage");
			addImageButton.ScriptBlock = "function FTB_OpenImageWindow(ftbName) { openWin('helpers/images.aspx',600,530,ftbName); }" + System.Environment.NewLine;
			addImageButton.ScriptBlock += @"function FTB_AddImage(ftbName,src,h,w){ FTB_InsertText(ftbName,'<img src=""' + src + '"" height=""' + h + '"" border=""0"" />');}";

			ToolbarButton addDocumentButton = new ToolbarButton("Insert a Document","FTB_OpenDocumentWindow","insertdocument");
			addDocumentButton.ScriptBlock = "function FTB_OpenDocumentWindow(ftbName) { openWin('helpers/documents.aspx',400,400,ftbName); }" + System.Environment.NewLine;
			addDocumentButton.ScriptBlock += @"function FTB_AddDocument(ftbName,text){ FTB_InsertText(ftbName,text);}";

			ToolbarButton addLinkButton = new ToolbarButton("Insert an Internal Link","FTB_OpenLinkWindow","insertlink");
			addLinkButton.ScriptBlock = "function FTB_OpenLinkWindow(ftbName) { openWin('helpers/links.aspx',400,400,ftbName); }" + System.Environment.NewLine;
			//addLinkButton.ScriptBlock += @"function FTB_AddLink(ftbName,text){ FTB_InsertText(ftbName,text);}";
			addLinkButton.ScriptBlock += @"function FTB_AddLink(ftbName,start,end){ FTB_SurroundText(ftbName,start,end);}";

			tb.Items.Add(addImageButton);
			tb.Items.Add(addDocumentButton);
			tb.Items.Add(addLinkButton);
			//tb.Items.Add(newImageGallery);

			

			this.Toolbars.Add(tb);
			this.ToolbarStyleConfiguration = ToolbarStyleConfiguration.NotSet;
			this.GutterBackColor = System.Drawing.ColorTranslator.FromHtml("#dddddd");
			this.BackColor = System.Drawing.ColorTranslator.FromHtml("#eeeeee");
			this.EditorBorderColorDark = System.Drawing.ColorTranslator.FromHtml("#999");
			this.EditorBorderColorLight = System.Drawing.ColorTranslator.FromHtml("#eee");
			this.SupportFolder = "~/admin/freetextbox/";
			this.HelperFilesPath = "~/admin/freetextbox/";
			this.ButtonFolder = "images";
			this.DesignModeCss = "../includes/ftb.css";
			//this.HelperFilesPath = "~/admin/freetextbox/";
			this.RenderMode = (FreeTextBoxControls.RenderMode)Enum.Parse(typeof(FreeTextBoxControls.RenderMode),System.Configuration.ConfigurationSettings.AppSettings["tb_defaultMode"],true);			


		}
/*
		
		protected override void OnInit(EventArgs e)
		{
		
			base.OnInit (e);
			
		}
		


		protected override void OnLoad(EventArgs e)
		{
			this.GutterBackColor = System.Drawing.ColorTranslator.FromHtml("#dddddd");
			this.BackColor = System.Drawing.ColorTranslator.FromHtml("#eeeeee");
			this.SupportFolder = "~/admin/freetextbox/";
			this.ButtonFolder = "custom";
			this.DesignModeCss = "../includes/main.css";
			base.OnLoad (e);
		}



		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
		}

*/



	}
}
