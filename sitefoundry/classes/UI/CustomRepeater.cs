using System;
using System.Collections;
using System.Data;

namespace Dury.SiteFoundry.UI
{
	public class CustomRepeater : System.Web.UI.WebControls.Repeater
	{
		private ICollection ds;
		public ICollection TrueDataSource
		{
			get{return ds;}
			set
			{
				ds=value;
				this.totalItems = ds.Count;
			}
		}
		private int totalItems;
		public int TotalItems
		{
			get{return totalItems;}
			set{totalItems=value;}
		}
		private int currentPage = 0;
		public int CurrentPage
		{
			get{return currentPage;}
			set{currentPage=value;}
		}
		private int pageSize = 10;
		public int PageSize
		{
			get {return pageSize;}
			set {pageSize=value;}
		}
		private int startRecord = 0;
		public int StartRecord
		{
			get
			{
				return startRecord;
			}
		}
		public int PrevPage
		{
			get
			{
				if (this.getCurrentPage() >= 1) 
				{
					return this.currentPage-1;
				} 
				else
				{
					return -1;
				}
			}
		}

		public int NextPage
		{
			get
			{
				if (this.currentPage < this.totalItems/this.pageSize) 
				{
					return this.currentPage+1;
				} 
				else
				{
					return -1;
				}

			}
		}
		
		/*
		private string rowFilter;
		public string RowFilter
		{
			get{return rowFilter;}
			set{rowFilter=value;}
		}
		private string sortBy;
		public string SortBy
		{
			get{return sortBy;}
			set{sortBy=value;}
		}
		private string sortDirection;
		public string SortDirection
		{
			get{return sortDirection;}
			set{sortDirection=value;}
		}
		*/


		public CustomRepeater()
		{
		}

		public void Bind()
		{
			System.Collections.ArrayList c = new System.Collections.ArrayList();
			int counter = 0;
			this.totalItems = this.ds.Count;
			this.startRecord = this.getCurrentPage() * this.pageSize;
			foreach (object o in this.ds)
			{
				if (counter >= this.startRecord && counter < this.startRecord + this.pageSize)
				{
					c.Add(o);
				}

				counter++;
			}
			this.DataSource = c;
			this.DataBind();
		}

		public void ChangePage(object o, System.EventArgs e)
		{
			if (((System.Web.UI.WebControls.Button)o).CommandName == "prev")
			{
				this.currentPage--;
			}
			if (((System.Web.UI.WebControls.Button)o).CommandName == "next")
			{
				this.currentPage++;
			}
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			this.currentPage = getCurrentPage();
			base.Render (writer);
		}


		private int getCurrentPage()
		{
			DuryTools.Utils.CustomQueryString qs = DuryTools.Utils.QueryStringUtils.QueryStringBuilder(Context.Request.RawUrl);
			if (qs["page"] != null)
			{
				return int.Parse(qs["page"]);
			}
			else
			{
				return 0;
			}
		}
	}
}
