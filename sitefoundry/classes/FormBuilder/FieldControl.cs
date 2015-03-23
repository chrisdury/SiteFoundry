using System;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Summary description for FormBuilderFieldControl.
	/// </summary>
	public class FieldControl : System.Web.UI.UserControl, IFieldControl
	{
		public Field FormField;
		public string Mode;
		public string action
		{
			get { return System.Web.HttpContext.Current.Request.QueryString["action"]; }
		}
		public virtual string GetValue()
		{
			return "get value not implemented in this control";
		}
		public virtual bool IsValid()
		{
			return false;
		}

		public FieldControl()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}


	interface IFieldControl
	{
		string GetValue();
	}
}
