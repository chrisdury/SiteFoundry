namespace Dury.SiteFoundry.nodeTemplates
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Dury.SiteFoundry;
	using DuryTools.Data;

	/// <summary>
	///		Summary description for linkDisplay.
	/// </summary>
	public class linkDisplay : Dury.SiteFoundry.UI.BaseControl
	{
		private DAL dal = new DAL();
		private string publicSuffix = "";
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			Node currentNode = (Node)Context.Items["currentNode"];
			if (!SFGlobal.IsUserCMS()) publicSuffix = "_public";			
            string linkLocation = (string)dal.execScalar("SELECT linkURL FROM Links" + publicSuffix + " WHERE nodeID = " + currentNode.Id);
			if (linkLocation != null)
			{
				Response.Redirect(linkLocation);
			} 
			else 
			{
				//Response.Write("Uh oh! problem with links");
				//throw new ErrorHandler("Problem loading node (type=links). Most likely caused by unpublished Link");
			}
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
