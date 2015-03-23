using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class Field
	{
		private int fieldID;
		private int sectionID;
		private int fieldTypeID;
		private string title;
		private string groupName;
		private string description;
		private bool isRequired;
		private bool isValidated;
		private int rank;
		private int height;
		private int width;
		private int rows;
		private int maxLength;
		private string listItems;
		private string validationExpression;
		private ObjectHolder sectionIDObject; // Strongly Type as FormSection if not Lazy-Loading
		private IList responses; // Supports both ObjectSet and Lazy-Loaded ObjectList
		//private IList fieldItems;

		public int FieldID
		{
			get { return this.fieldID; }
		}

		public int SectionID
		{
			get { return this.sectionID; }
			set { this.sectionID = value; }
		}

		public int FieldTypeID
		{
			get { return this.fieldTypeID; }
			set { this.fieldTypeID = value; }
		}

		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		public string GroupName
		{
			get { return this.groupName; }
			set { this.groupName = value; }
		}

		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

		public bool IsRequired
		{
			get { return this.isRequired; }
			set { this.isRequired = value; }
		}

		public bool IsValidated
		{
			get { return this.isValidated; }
			set { this.isValidated = value; }
		}

		public string ValidationExpression
		{
			get { return this.validationExpression; }
			set { this.validationExpression = value; }
		}

		public int Rank
		{
			get { return this.rank; }
			set { this.rank = value; }
		}

		public int Height
		{
			get { return this.height; }
			set { this.height = value; }
		}

		public int Width
		{
			get { return this.width; }
			set { this.width = value; }
		}

		public int Rows
		{
			get { return this.rows; }
			set { this.rows = value; }
		}

		public int MaxLength
		{
			get { return this.maxLength; }
			set { this.maxLength = value; }
		}

		public string ListItems
		{
			get { return this.listItems; }
			set { this.listItems = value; }
		}



		// Return the primary key property from the primary key object
		public FormSection SectionIDObject
		{
			get { return (FormSection)this.sectionIDObject.InnerObject; }
		}

		public IList Responses
		{
			get { return this.responses; }
			set { this.responses = value; }
		}

		/*

		public IList FieldItems
		{
			get { return this.fieldItems; }
			set { this.fieldItems = value; }
		}
		
		*/


	}
}
