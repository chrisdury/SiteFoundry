using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class Responder
	{
		private int responderID;
		private int formID;
		private string email;
		private string firstname;
		private string lastname;
		private string address1;
		private string address2;
		private string city;
		private string state;
		private string country;
		private string postalcode;
		private string username;
		private string password;
		private DateTime dateCreated;
		private IList responses; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private ObjectHolder formObject; 

		public int ResponderID
		{
			get { return this.responderID; }
			set { responderID = value; }
		}
		public int FormID
		{
			get { return this.formID; }
			set { formID = value; }
		}

		public string Email
		{
			get { return this.email; }
			set { this.email = value; }
		}
		public string Firstname
		{
			get { return this.firstname; }
			set { this.firstname = value; }
		}

		public string Lastname
		{
			get { return this.lastname; }
			set { this.lastname = value; }
		}

		public string Address1
		{
			get { return this.address1; }
			set { this.address1 = value; }
		}

		public string Address2
		{
			get { return this.address2; }
			set { this.address2 = value; }
		}

		public string City
		{
			get { return this.city; }
			set { this.city = value; }
		}

		public string State
		{
			get { return this.state; }
			set { this.state = value; }
		}

		public string Country
		{
			get { return this.country; }
			set { this.country = value; }
		}

		public string Postalcode
		{
			get { return this.postalcode; }
			set { this.postalcode = value; }
		}

		public string Username
		{
			get { return this.username; }
			set { this.username = value; }
		}

		public string Password
		{
			get { return this.password; }
			set { this.password = value; }
		}

		public DateTime DateCreated
		{
			get { return this.dateCreated; }
			set { this.dateCreated = value; }
		}

		public IList Responses
		{
			get { return this.responses; }
			set { responses = value; }
		}
		// Return the primary key property from the primary key object
		public Form FormObject
		{
			get { return (Form)this.formObject.InnerObject; }
		}
	}
}
