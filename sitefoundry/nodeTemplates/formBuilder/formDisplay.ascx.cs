namespace Dury.SiteFoundry.nodeTemplates.formBuilder
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Dury.SiteFoundry;

	/// <summary>
	///		Summary description for formDisplay.
	/// </summary>
	public class formDisplay : Dury.SiteFoundry.UI.BaseControl
	{
		protected PlaceHolder formHolder;
		protected MetaBuilders.WebControls.CustomForm form;
		protected Button submitButton;

		private Node currentNode;
		private Form currentForm;

		private void Page_Load(object sender, System.EventArgs e)
		{
			form.Action = Request.RawUrl;
			currentNode = (Node)Context.Items["currentNode"];
			submitButton.Click += new EventHandler(submitButton_Click);

			currentForm = FormBuilder.GetForm(currentNode);
			//if (!IsPostBack) 
			//{
				loadForm();
			//}

		}

		private void loadForm()
		{
			
			if (currentNode.FormNodeIDs.Count == 1)
			{
				formHolder.Controls.Clear();




				if (currentForm.IsActive)
				{
					foreach(FormSection fs in currentForm.FormSections)
					{
						string s = "<fieldset><legend>" + fs.Title + "</legend>";
						if (currentForm.DisplaySections) formHolder.Controls.Add(new LiteralControl(s));
						foreach(Field f in fs.FieldSectionIDs)
						{
							formHolder.Controls.Add(getFieldDisplayControl(f));
						}
						if (currentForm.DisplaySections) formHolder.Controls.Add(new LiteralControl("</fieldset>"));
					}
				}
				else
				{
					Controls.Add(new LiteralControl("This form is inactive"));
					form.Visible = false;
				}
			}
		}


		// put these in FormBuilder
		private FieldControl getFieldControl(Field f, bool isDisplayControl)
		{
			FieldTypeInfo fti = FormBuilder.GetFieldTypeInfo(f.FieldTypeID);
			string s = (isDisplayControl) ? fti.DisplaySrc : fti.EditSrc;
			FieldControl fc = (FieldControl)Page.LoadControl("~/nodeTemplates/formBuilder/" + s);
			fc.FormField = f;
			fc.Mode = "Display";//(Request.QueryString["action"] != null) ? Request.QueryString["action"] : "";
			return fc;
		}
		private FieldControl getFieldDisplayControl(Field f)
		{
			return getFieldControl(f,true);
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

		private void submitButton_Click(object sender, EventArgs e)
		{
			bool b = FormBuilder.ProcessForm(this.currentForm,formHolder.Controls);
			if (b)
			{
				formHolder.Controls.Clear();
				submitButton.Visible = false;
				formHolder.Controls.Add(new LiteralControl(currentForm.SuccessText));
			}
			else 
			{
				submitButton.Visible = false;
				formHolder.Controls.Add(new LiteralControl("SHIT!"));
			}

		}
	}
}
