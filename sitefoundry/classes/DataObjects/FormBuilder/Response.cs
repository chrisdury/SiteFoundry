using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class Response
	{
		private int answerID;
		private int responderID;
		private int fieldID;
		private string value;
		private DateTime dateCreated;
		private ObjectHolder fieldObject; // Strongly Type as Field if not Lazy-Loading
		private ObjectHolder responderObject; // Strongly Type as Responder if not Lazy-Loading

		public int AnswerID
		{
			get { return this.answerID; }
			set { answerID = value; }
		}

		public int ResponderID
		{
			get { return this.responderID; }
			set { this.responderID = value; }
		}

		public int FieldID
		{
			get { return this.fieldID; }
			set { this.fieldID = value; }
		}

		public string Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		public DateTime DateCreated
		{
			get { return this.dateCreated; }
			set { this.dateCreated = value; }
		}

		// Return the primary key property from the primary key object
		public Field FieldObject
		{
			get { return (Field)this.fieldObject.InnerObject; }
		}

		// Return the primary key property from the primary key object
		public Responder ResponderObject
		{
			get { return (Responder)this.responderObject.InnerObject; }
		}

	}
}
