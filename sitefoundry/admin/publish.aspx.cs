using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

using Dury.SiteFoundry;
using Dury.SiteFoundry.Security;
using DuryTools.Data;


namespace Dury.SiteFoundry.Admin
{
	/// <summary>
	/// Summary description for publish.
	/// </summary>
	public class publish : DuryTools.UI.BasePage
	{
		protected System.Web.UI.WebControls.CheckBoxList nodesToPublish;
        protected System.Web.UI.WebControls.PlaceHolder itemsHolder;
		protected System.Web.UI.WebControls.DataGrid itemsGrid;
		protected System.Web.UI.WebControls.Button publishButton;
		protected System.Web.UI.WebControls.Literal results;
		protected System.Web.UI.WebControls.Literal info;
		private string nodeXmlFile;
		private HybridDictionary nodeInfo;

		protected UserPrincipal user;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this.PageTitle = "Publish";
			user = (UserPrincipal)Context.User;

			//if (user.IsInRole(SFGlobal.AdminstratorRoleName) || user.IsInRole(SFGlobal.PublisherRoleName))
			if (user.IsUserAdmin())
			{
				init();
				if (!IsPostBack) 
				{
					oneTimeInit();
				}
			}
			else
			{
				publishButton.Visible = false;
				nodesToPublish.Visible = false;
				info.Text = "You don't have permission to publish";
			}

		}

		private void init()
		{
			nodeXmlFile = SFGlobal.BaseDirectory + System.Configuration.ConfigurationSettings.AppSettings["nodeTemplateDefinitions"];
			nodeInfo  = getNodeInfo(nodeXmlFile);
			getItemsForPublish();
		}


		private void oneTimeInit()
		{
            fillPublishOptionsList();
			           
		}



		private void getItemsForPublish()
		{
			if (nodeInfo.Count == 0) return;
			DataTable dt = new DataTable();
			dt.Columns.Add("type",typeof(string));
			dt.Columns.Add("nodeID",typeof(int));
			dt.Columns.Add("filename",typeof(string));
			dt.Columns.Add("title",typeof(string));
			dt.Columns.Add("modifiedBy",typeof(string));
			dt.Columns.Add("dateModified",typeof(DateTime));

			foreach(DictionaryEntry de in nodeInfo)
			{
				publishNodeInfo pni = (publishNodeInfo)de.Value;
				DataSet ds = SFGlobal.DAL.execDataSet(pni.SelectSP,null);
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					dt.Rows.Add(new object[] { pni.Name,(int)dr["nodeID"],dr["filename"].ToString(),dr["title"].ToString(),dr["username"].ToString(),(DateTime)dr["dateModified"]});
				}
			}
			dt.DefaultView.Sort = "dateModified DESC";
			itemsGrid.DataSource = dt.DefaultView;
			itemsGrid.DataBind();

			if (dt.Rows.Count == 0) itemsGrid.Visible = false;
		}


		private HybridDictionary getNodeInfo(string xmlFile)
		{
			HybridDictionary hd = new HybridDictionary();
			XmlDocument xd = new XmlDocument();
			try 
			{
				xd.Load(xmlFile);
				foreach(XmlNode xn in xd["nodeTypes"])
				{
					if (xn.Attributes["publishSP"] != null && xn.Attributes["selectSP"] != null) 
					{
						publishNodeInfo pni = new publishNodeInfo();
						pni.Name = xn.Attributes["name"].Value;
						pni.PublishSP = xn.Attributes["publishSP"].Value;
						pni.SelectSP = xn.Attributes["selectSP"].Value;
						hd.Add(pni.Name,pni);
					}
				}
			}
			catch(Exception e)
			{
				throw new DuryTools.ErrorHandler("can't read xml..."+xmlFile,e);
			}
			return hd;
		}

		private void fillPublishOptionsList()
		{
			foreach(DictionaryEntry de in nodeInfo)
			{
				ListItem li = new ListItem(de.Key.ToString());
				li.Selected = true;
				nodesToPublish.Items.Add(li);
			}
		}

		private void publishItems()
		{
			string s = "";
			// first publish the nodes
			SFGlobal.DAL.execNonQuery("publishNodes",null);
			s += "published nodes<br>";

			// now publish what we want...
			foreach(ListItem li in nodesToPublish.Items)
			{
				if (li.Selected) 
				{
					publishNodeInfo pni = (publishNodeInfo)nodeInfo[li.Value];
					SFGlobal.DAL.execNonQuery(pni.PublishSP,null);
					s+= "published " + pni.Name + "<BR>";
				}
			}
			SFGlobal.UpdateNodes();
			getItemsForPublish();
			results.Text = s;
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
			publishButton.Click +=new EventHandler(publishButton_Click);
		}
		#endregion
		private void publishButton_Click(object sender, EventArgs e)
		{
			publishItems();			
		}
	}

	public struct publishNodeInfo
	{
		public string Name;
		public string PublishSP;
		public string SelectSP;
	}



}
