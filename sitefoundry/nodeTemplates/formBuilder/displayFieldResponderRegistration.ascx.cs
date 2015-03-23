namespace Dury.SiteFoundry.nodeTemplates.formBuilder
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Xml;

	/// <summary>
	///		Summary description for displayFieldResponderRegistration.
	/// </summary>
	public class displayFieldResponderRegistration : Dury.SiteFoundry.FieldControl
	{
		protected Literal title;
		protected Literal description;

		public TextBox email;
		public TextBox firstname;
		public TextBox lastname;
		public TextBox address1;
		public TextBox address2;
		public TextBox city;
		public DropDownList stateprov;
		public TextBox postcode;

		protected RequiredFieldValidator rfv_email;
		protected RequiredFieldValidator rfv_firstname;
		protected RequiredFieldValidator rfv_lastname;
		protected RequiredFieldValidator rfv_address;
		protected RequiredFieldValidator rfv_city;
		protected RequiredFieldValidator rfv_postcode;
		protected CompareValidator cv_stateprov;

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			title.Text = this.FormField.Title;
			description.Text = FormField.Description;
			if (Mode == "Display") 
			{
				if (FormField.IsRequired) 
				{
					foreach(BaseValidator bv in Page.Validators)
					{
						bv.Enabled = true;
					}
				}
			}

			//if (!IsPostBack)
			//{
				fillStateProv();
			//}

		}

		private void fillStateProv()
		{
			System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
			xd.Load(MapPath("stateprov.xml"));
			XmlNodeList xnl2 = xd.SelectNodes("/provlist/country[@name='Canada']/*");
			foreach(XmlNode xn in xnl2) 
			{
				stateprov.Items.Add(new ListItem(xn.Attributes["name"].Value,xn.Attributes["code"].Value));
			}
			XmlNodeList xnl = xd.SelectNodes("/provlist/country[@name='United States']/*");
			foreach(XmlNode xn in xnl) 
			{
				stateprov.Items.Add(new ListItem(xn.Attributes["name"].Value,xn.Attributes["code"].Value));
			}			
			stateprov.Items.Insert(0,new ListItem("select...","0"));
            
		}

		public override string GetValue()
		{
			if (Page.IsValid)
				return "Responder Registration";
			else
				throw new Exception("control isn't valid :" + this.ID);
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
