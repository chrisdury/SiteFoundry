using System;
using System.Collections;
using System.Collections.Specialized;

namespace Dury.SiteFoundry.Utils
{
	/// <summary>
	/// Summary description for QueryString.
	/// </summary>
	public class QueryString : NameValueCollection
	{
		public QueryString(NameValueCollection source)
		{
			foreach(string s in source.AllKeys)
			{
				this.Add(s,source[s]);
			}
		}

		public override string ToString()
		{
			string output = String.Empty;
			foreach(string s in this.AllKeys)
			{
				output += s + "=" + this[s] + "&";
			}
			return output.Substring(0,output.Length-1);
		}




	}
}
