using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class FormSection
	{
		private int formSectionID;
		private int formID;
		private string title;
		private string description;
		private int rank;
		private ObjectHolder formObject; // Strongly Type as FbForm if not Lazy-Loading
		private IList fieldSectionIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList

		public int FormSectionID
		{
			get { return this.formSectionID; }
		}

		public int FormID
		{
			get { return this.formID; }
			set { this.formID = value; }
		}

		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}
		public int Rank
		{
			get { return this.rank; }
			set { this.rank= value; }
		}

		// Return the primary key property from the primary key object
		public Form FormObject
		{
			get { return (Form)this.formObject.InnerObject; }
		}

		public IList FieldSectionIDs
		{
			get { return this.fieldSectionIDs; }
			set { this.fieldSectionIDs = value; }
		}

	}
}
