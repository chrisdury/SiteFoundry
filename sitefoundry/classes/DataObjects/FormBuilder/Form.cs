using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class Form
	{
		private int formID;
		private int nodeID;
		private string title;
		private string description;
		private string successText;
		private bool isActive;
		private bool displaySections;
		private ObjectHolder nodeIDObject; // Strongly Type as Node if not Lazy-Loading
		private IList formSections; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList responders; // Supports both ObjectSet and Lazy-Loaded ObjectList


		public int FormID
		{
			get { return this.formID; }
		}

		public int NodeID
		{
			get { return this.nodeID; }
			set { this.nodeID = value; }
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
		public string SuccessText
		{
			get { return this.successText; }
			set { this.successText = value; }
		}

		public bool IsActive
		{
			get { return this.isActive; }
			set { this.isActive = value; }
		}
		public bool DisplaySections
		{
			get { return this.displaySections; }
			set { this.displaySections = value; }
		}

		// Return the primary key property from the primary key object
		public Node NodeIDObject
		{
			get { return (Node)this.nodeIDObject.InnerObject; }
		}

		public IList FormSections
		{
			get { return this.formSections; }
			set { this.formSections = value; }
		}

		public IList Responders
		{
			get { return this.responders; }
			set { this.responders = value; }
		}

	}
}
