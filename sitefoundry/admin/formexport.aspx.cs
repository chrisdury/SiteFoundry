using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Dury.SiteFoundry;
using Wilson.ORMapper;

namespace Dury.SiteFoundry.admin
{
	/// <summary>
	/// Summary description for formexport.
	/// </summary>
	public class formexport : DuryTools.UI.BasePage
	{
		
		protected DataGrid formsGrid;

		private void Page_Load(object sender, System.EventArgs e)
		{
			PageTitle = "Form Data Export";
			
			formsGrid.PageIndexChanged += new DataGridPageChangedEventHandler(formsGrid_PageIndexChanged);
			formsGrid.SortCommand += new DataGridSortCommandEventHandler(formsGrid_SortCommand);
			formsGrid.EditCommand += new DataGridCommandEventHandler(formsGrid_EditCommand);
			formsGrid.CancelCommand += new DataGridCommandEventHandler(formsGrid_CancelCommand);
			formsGrid.ItemDataBound += new DataGridItemEventHandler(formsGrid_ItemDataBound);

			if (!IsPostBack) 
			{
				fillForms();
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion


		#region FORMSGRID
		private void fillForms()
		{
			ObjectSet os;
			if (ViewState["sortby"] != null) 
				os = FormBuilder.GetAllForms(ViewState["sortby"].ToString(),ViewState["sortDirection"].ToString());
			else
				os = FormBuilder.GetAllForms();
			if (os.Count == 0) 
			{
				formsGrid.Visible = false;
			} 
			else 
			{
				formsGrid.Visible = true;
				formsGrid.DataSource = os;
				formsGrid.DataBind();
			}

		}

		private void formsGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			formsGrid.CurrentPageIndex = e.NewPageIndex;
			fillForms();

		}

		private void formsGrid_SortCommand(object source, DataGridSortCommandEventArgs e)
		{
			if (ViewState["sortby"] != null) 
			{
				
				if ((string)ViewState["sortby"] == (string)e.SortExpression)
				{
					ViewState["sortDirection"] = ((string)ViewState["sortDirection"] == "ASC") ? "DESC" : "ASC";
				
				} 
				else 
				{
					ViewState["sortDirection"] = "ASC";
					ViewState["sortby"] = (string)e.SortExpression;
				}
			} 
			else 
			{
				ViewState["sortDirection"] = "ASC";
				ViewState["sortby"] = (string)e.SortExpression;
			}
			fillForms();

		}

		private void formsGrid_EditCommand(object source, DataGridCommandEventArgs e)
		{
			formsGrid.EditItemIndex = e.Item.ItemIndex;
			fillForms();
		}

		private void formsGrid_CancelCommand(object source, DataGridCommandEventArgs e)
		{
			formsGrid.EditItemIndex = -1;
			fillForms();
		}


		public string GetResponseCount(int formID)
		{
            SelectProcedure sp = new SelectProcedure(typeof(int),"fb_sp_GetResponderCount");
			sp.AddParameter("@formID",formID);
			return ((int)SFGlobal.ObjectManager.ExecuteScalar(sp)).ToString();
		}
		private void formsGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
			{
				if (e.Item.FindControl("holder") != null)
				{
					Form f = (Form)e.Item.DataItem;
					PlaceHolder ph = (PlaceHolder)e.Item.FindControl("holder");

					DataGrid dg = new DataGrid();
					dg.DataSource = FormBuilder.GetRespondersForExport(f);
					dg.DataBind();
					ph.Controls.Add(dg);
				}

			}

		}
		#endregion


	}
}
