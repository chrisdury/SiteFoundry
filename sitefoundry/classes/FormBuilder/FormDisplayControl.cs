using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dury.SiteFoundry;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Summary description for FormDisplayControl.
	/// </summary>
	public class FormDisplayControl : Control, INamingContainer
	{
		public Node CurrentNode;
		//protected MetaBuilders.WebControls.CustomForm form;
		private Form currentForm;

		
		public FormDisplayControl()
		{
		}

		protected override void OnLoad(EventArgs e)
		{
			//base.OnLoad (e);
			currentForm = FormBuilder.GetForm(CurrentNode);
		}



		protected override void CreateChildControls()
		{
			if (CurrentNode.FormNodeIDs.Count == 1)
			{
				if (currentForm.IsActive)
				{
					MetaBuilders.WebControls.CustomForm cf = new MetaBuilders.WebControls.CustomForm();
					cf.Action = this.Page.Request.RawUrl;
					cf.ID = "form";
					
					foreach(FormSection fs in currentForm.FormSections)
					{
						string s = "<fieldset><legend>" + fs.Title + "</legend>";
						if (currentForm.DisplaySections) cf.Controls.Add(new LiteralControl(s));
						foreach(Field f in fs.FieldSectionIDs)
						{
							cf.Controls.Add(getFieldDisplayControl(f));
						}
						if (currentForm.DisplaySections) cf.Controls.Add(new LiteralControl("</fieldset>"));
					}
					Button b = new Button();
					b.Text = "submit";
					b.Click += new EventHandler(b_Click);
					cf.Controls.Add(b);

					this.Controls.Add(cf);
				}
				else
				{
					this.Controls.Add(new LiteralControl("This form is inactive"));
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

		private void b_Click(object sender, EventArgs e)
		{
			MetaBuilders.WebControls.CustomForm cf = (MetaBuilders.WebControls.CustomForm)this.FindControl("form");
			bool b = FormBuilder.ProcessForm(this.currentForm,cf.Controls);
			if (b)
			{
				this.Controls.Clear();
				this.Controls.Add(new LiteralControl(currentForm.SuccessText));
				//formHolder.Controls.Clear();
				//submitButton.Visible = false;
				//formHolder.Controls.Add(new LiteralControl(currentForm.SuccessText));
			}
			else 
			{
                
				//submitButton.Visible = false;
				//formHolder.Controls.Add(new LiteralControl("SHIT!"));
			}
		}
	}
}
