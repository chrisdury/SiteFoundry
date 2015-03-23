using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class FieldItem
	{
		private int id;
		private int fieldID;
		private string text;
		private int rank;
		private ObjectHolder fieldObject; // Strongly Type as FbField if not Lazy-Loading

		public int Id
		{
			get { return this.id; }
		}

		public int FieldID
		{
			get { return this.fieldID; }
			set { this.fieldID = value; }
		}

		public string Text
		{
			get { return this.text; }
			set { this.text = value; }
		}

		public int Rank
		{
			get { return this.rank; }
			set { this.rank = value; }
		}

		// Return the primary key property from the primary key object
		public Field FieldObject
		{
			get { return (Field)this.fieldObject.InnerObject; }
		}

	}
}
