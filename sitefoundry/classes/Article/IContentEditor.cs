using System;

namespace Dury.SiteFoundry.Article
{
	/// <summary>
	/// Summary description for IContentEditor.
	/// </summary>
	interface IContentEditor
	{
		string Text 
		{
			get;
			set;
		}
		int Height 
		{
			get;
			set;
		}
		int Width 
		{
			get;
			set;
		}
	}
}
