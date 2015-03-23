using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Dury.SiteFoundry;
using Wilson.ORMapper;
using Dury.SiteFoundry.nodeTemplates.formBuilder;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Summary description for FormBuilder.
	/// </summary>
	public class FormBuilder
	{

		#region CONSTRUCTOR
		private static readonly FormBuilder instance=new FormBuilder();
		public static FormBuilder GetInstance() { return instance; }
		static FormBuilder() {}
		FormBuilder()
		{
		}

		#endregion

		#region FORM
		public static Form GetForm()
		{
			return (Form)SFGlobal.ObjectManager.GetObject(typeof(Form));
		}

		public static Form GetForm(Node currentNode)
		{
			OPathQuery oq = new OPathQuery(typeof(Form),"NodeID==" + currentNode.Id,"");
			Form f = (Form)SFGlobal.ObjectManager.GetObject(oq);
			if (f!=null) return f;
			else 
			{
				f = GetForm();
				f.NodeID = currentNode.Id;
				return f;
			}
		}

		public static void DeleteForm(Node currentNode)
		{
			OPathQuery oq = new OPathQuery(typeof(Form),"NodeID==" + currentNode.Id,"");
			Form f = (Form)SFGlobal.ObjectManager.GetObject(oq);
			if (f != null) 
			{
				SFGlobal.ObjectManager.MarkForDeletion(f);
				SFGlobal.ObjectManager.PersistChanges(f);
			}
		}

		public static Form GetForm(int formID)
		{
			return (Form)SFGlobal.ObjectManager.GetObject(typeof(Form),formID);
		}
		public static void SaveForm(Form f)
		{
			SFGlobal.ObjectManager.PersistChanges(f);
			SFGlobal.ObjectManager.Resync(f);
			SFGlobal.AlertUser("Form Saved");
		}


		public static ObjectSet GetAllForms()
		{
			return GetAllForms("","");			
		}

		public static ObjectSet GetAllForms(string sortBy, string sortDirection)
		{
			ObjectQuery oq = new ObjectQuery(typeof(Form),"",sortBy + sortDirection);
			return SFGlobal.ObjectManager.GetObjectSet(oq);
		}


		#endregion

		#region SECTION
		public static FormSection GetFormSection()
		{
			return (FormSection)SFGlobal.ObjectManager.GetObject(typeof(FormSection));
		}
		public static FormSection GetFormSection(int formSectionID)
		{
			return (FormSection)SFGlobal.ObjectManager.GetObject(new OPathQuery(typeof(FormSection),"FormSectionID==" + formSectionID,""));
		}
		public static FormSection AddSection(Form f, string title, string description)
		{
			FormSection fs = (FormSection)SFGlobal.ObjectManager.GetObject(typeof(FormSection));
			fs.FormID = f.FormID;
			fs.Title = title;
			fs.Description = description;
			fs.Rank = getSectionNextRank(f);
			SFGlobal.ObjectManager.PersistChanges(fs);
			SFGlobal.ObjectManager.Resync(fs);
			SFGlobal.AlertUser("Section: " + fs.Title + " Added");
			return fs;
		}

		private static int getSectionNextRank(Form f) 
		{
			return f.FormSections.Count + 1;
		}

		public static void ChangeSectionRank(FormSection fs, int dir)
		{
			int i = getSectionIndexInParentCollection(fs);
			if (i >= 0 && i < fs.FormObject.FormSections.Count) 
			{
				int oldRank = fs.Rank;
				if (dir == -1 && i == 0) return;
				if (dir == 1 && i == fs.FormObject.FormSections.Count-1) return;
				fs.Rank = ((FormSection)fs.FormObject.FormSections[i+dir]).Rank;
				((FormSection)fs.FormObject.FormSections[i+dir]).Rank = oldRank;
				SFGlobal.ObjectManager.PersistChanges(fs);
				SFGlobal.ObjectManager.PersistChanges(fs.FormObject.FormSections[i+dir]);
				SFGlobal.ObjectManager.Resync(fs.FormObject);
				SFGlobal.ObjectManager.Resync(fs);
				SFGlobal.ObjectManager.Resync(fs.FormObject.FormSections[i+dir]);

			}
			SFGlobal.AlertUser("Changed Section: " + fs.Title + " rank");
		}

		private static int getSectionIndexInParentCollection(FormSection fs)
		{
			int i=0;
			foreach(FormSection formSection in fs.FormObject.FormSections)
			{
				if (fs.FormSectionID == formSection.FormSectionID)
					return i;
				i++;
			}
			throw new Exception("can't find formsection in parent collection");
		}

		public static void SaveFormSection(FormSection fs)
		{
			SFGlobal.ObjectManager.PersistChanges(fs);
			SFGlobal.ObjectManager.Resync(fs);
			SFGlobal.AlertUser("Section: " + fs.Title + " Saved");
		}

		public static void RemoveSection(FormSection fs) 
		{
			SFGlobal.ObjectManager.MarkForDeletion(fs);
			SFGlobal.ObjectManager.PersistChanges(fs);
			SFGlobal.ObjectManager.Resync(fs.FormObject);
			SFGlobal.AlertUser("Section: " + fs.Title + " Deleted");
		}

		#endregion

		#region FIELD		
		public static Field GetField()
		{
			return (Field)SFGlobal.ObjectManager.GetObject(typeof(Field));
		}
		public static Field GetField(int fieldID)
		{
			return (Field)SFGlobal.ObjectManager.GetObject(new OPathQuery(typeof(Field),"FieldID==" + fieldID,""));
		}
		public static Field AddFieldToSection(FormSection fs, int fieldType, string title)
		{
			return AddFieldToSection(fs,fieldType,title,"","",false,false,"");
		}
		public static Field AddFieldToSection(FormSection fs, int fieldType, string title, string groupName, string description, bool isRequired, bool isValidated, string validationExpression)
		{
			Field f = (Field)SFGlobal.ObjectManager.GetObject(typeof(Field));
			f.SectionID = fs.FormSectionID;
			f.FieldTypeID = fieldType;
			f.Title = title;
			f.Description = description;
			f.GroupName = groupName;
			f.IsRequired = isRequired;
			f.IsValidated = isValidated;
			f.ValidationExpression = validationExpression;
			f.Rank = getFieldNextRank(fs);
			SFGlobal.ObjectManager.PersistChanges(f);
			SFGlobal.ObjectManager.Resync(fs);
			SFGlobal.AlertUser("Field added");
			return f;
		}
		public static void SaveField(Field f)
		{
			SFGlobal.ObjectManager.PersistChanges(f);
			SFGlobal.ObjectManager.Resync(f);
			SFGlobal.AlertUser("Saved Field: " + f.Title);
		}

		private static int getFieldNextRank(FormSection fs)
		{
			return fs.FieldSectionIDs.Count+1;
		}

		public static void ChangeFieldRank(Field f, int dir)
		{

			int i = getFieldIndexInParentCollection(f);
			if (i >= 0 && i < f.SectionIDObject.FieldSectionIDs.Count) 
			{
				int oldRank = f.Rank;
				if (dir == -1 && i == 0) return;
				if (dir == 1 && i == f.SectionIDObject.FieldSectionIDs.Count-1) return;
				f.Rank = ((Field)f.SectionIDObject.FieldSectionIDs[i+dir]).Rank;
				((Field)f.SectionIDObject.FieldSectionIDs[i+dir]).Rank = oldRank;
				SFGlobal.ObjectManager.PersistChanges(f);
				SFGlobal.ObjectManager.PersistChanges(f.SectionIDObject.FieldSectionIDs[i+dir]);
				SFGlobal.ObjectManager.Resync(f);
				SFGlobal.ObjectManager.Resync(f.SectionIDObject.FieldSectionIDs[i+dir]);
				SFGlobal.ObjectManager.Resync(f.SectionIDObject);
			}
			SFGlobal.AlertUser("Changed Field: " + f.Title + " Rank");
		}

		private static int getFieldIndexInParentCollection(Field f)
		{
			int i=0;
			foreach(Field field in f.SectionIDObject.FieldSectionIDs)
			{
				if (f.FieldID == field.FieldID)
					return i;
				i++;
			}
			throw new Exception("can't find field in parent collection");

		}

		public static void RemoveField(Field f)
		{
			SFGlobal.ObjectManager.MarkForDeletion(f);
			SFGlobal.ObjectManager.PersistChanges(f);
			SFGlobal.AlertUser("Field: " + f.Title + " Deleted!");
		}


		public static ArrayList GetFieldsInForm(Form f)
		{
			DataSet ds = SFGlobal.DAL.execDataSet("fb_sp_GetFieldsInForm",f.FormID);
			ArrayList al = new ArrayList();
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				al.Add(GetField((int)dr[0]));
			}
			return al;
		}


		#endregion

		#region PROCESS FORM

		public static bool ProcessForm(Form f, ControlCollection cc)
		{
			bool formDone = false;
			bool hasResponder = formHasResponder(f);

			Responder res = FormBuilder.GetResponder();
			if (hasResponder)
			{
				displayFieldResponderRegistration reg = getResponderControl(cc);
				res.FormID = f.FormID;
				res.Email = reg.email.Text;
				res.Firstname = reg.firstname.Text;
				res.Lastname = reg.lastname.Text;
				res.Address1 = reg.address1.Text;
				res.Address2 = reg.address2.Text;
				res.City = reg.city.Text;
				res.State = reg.stateprov.SelectedValue;
				res.Postalcode = reg.postcode.Text;
				//r.Country = reg.
				//r.Username;
				//r.Password;
				res.DateCreated = DateTime.Now;
				SaveResponder(res);
			} 
			else
			{
				res.Firstname = "anonymous";
				res.DateCreated = DateTime.Now;
				res.FormID = f.FormID;
				SaveResponder(res);
			}

			foreach(Control c in cc)
			{
				if (c.GetType().BaseType.BaseType == typeof(FieldControl))
				{
					Response r = GetResponse();
					FieldControl fc = (FieldControl)c;
					r.FieldID = fc.FormField.FieldID;
					r.ResponderID = r.ResponderID;
					r.Value = fc.GetValue();
					r.DateCreated = DateTime.Now;
					//if (hasResponder) 
					r.ResponderID = res.ResponderID;
					SaveResponse(r);
				}
				formDone = true;
			}

			return formDone;

		}

		private static displayFieldResponderRegistration getResponderControl(ControlCollection cc)
		{
			foreach(Control c in cc)
			{
				if (c.GetType().BaseType.BaseType == typeof(FieldControl))
				{
					FieldControl fc = (FieldControl)c;
					if (fc.FormField.FieldTypeID == 8) return (displayFieldResponderRegistration)fc;
				}
			}
			return null;
		}


		private static bool formHasResponder(Form f)
		{
			foreach(FormSection fs in f.FormSections)
				foreach(Field field in fs.FieldSectionIDs)
					if (field.FieldTypeID == 8) return true;
			return false;
		}

		#endregion

		#region FIELDTYPES
		public static ArrayList FieldTypesList
		{
			get 
			{
				if (fieldTypesList == null)  
				{
					fieldTypesList = new ArrayList();
					XmlDocument xd = new XmlDocument();
					xd.Load(SFGlobal.BaseDirectory + "nodeTemplates/formBuilder/fieldTypes.config");
					foreach(XmlNode xn in xd["fieldTypes"]) 
					{
						fieldTypesList.Add(new FieldTypeInfo(int.Parse(xn.Attributes["id"].Value),xn.Attributes["name"].Value,xn.Attributes["editSrc"].Value,xn.Attributes["displaySrc"].Value));
					}
				}
				return fieldTypesList;
			}
		}
		private static ArrayList fieldTypesList;
		public static FieldTypeInfo GetFieldTypeInfo(int fieldTypeID)
		{
			foreach(FieldTypeInfo fti in FieldTypesList)
			{
				if (fti.ID == fieldTypeID)
					return fti;
			}
			throw new Exception("cannot find fieldTypeID:" + fieldTypeID);
		}
		#endregion

		#region RESPONDER

		public static Responder GetResponder()
		{
			return (Responder)SFGlobal.ObjectManager.GetObject(typeof(Responder));
		}
		public static Responder GetResponder(int id)
		{
			return (Responder)SFGlobal.ObjectManager.GetObject(typeof(Responder),id);
		}

		public static ArrayList GetResponders(Form f)
		{
			return (ArrayList)SFGlobal.ObjectManager.GetCollection(typeof(ArrayList),typeof(Responder),"FormID=" + f.FormID);
		}


		public static void SaveResponder(Responder r)
		{
			SFGlobal.ObjectManager.PersistChanges(r);
			SFGlobal.ObjectManager.Resync(r);
		}
		public static void DeleteResponder(Responder r)
		{
			SFGlobal.ObjectManager.MarkForDeletion(r);
			SFGlobal.ObjectManager.PersistChanges(r);
		}
		#endregion

		#region RESPONSE

		public static Response GetResponse()
		{
			return (Response)SFGlobal.ObjectManager.GetObject(typeof(Response));
		}
		public static Response GetResponse(int id)
		{
			return (Response)SFGlobal.ObjectManager.GetObject(typeof(Response),id);
		}
		public static void SaveResponse(Response r)
		{
			SFGlobal.ObjectManager.PersistChanges(r);
			SFGlobal.ObjectManager.Resync(r);
		}
		public static void DeleteResponse(Response r)
		{
			SFGlobal.ObjectManager.MarkForDeletion(r);
			SFGlobal.ObjectManager.PersistChanges(r);
		}

		#endregion


		#region RESPONDER DATATABLE EXPORT


		public static DataTable GetRespondersForExport(Form f)
		{
			DataTable dt = new DataTable();
			ArrayList fields = GetFieldsInForm(f);
			
			bool hasResponder = formHasResponder(f);
			// create table schema
			if (hasResponder)
			{
				dt.Columns.Add("email",typeof(string));
				dt.Columns.Add("firstname",typeof(string));
				dt.Columns.Add("lastname",typeof(string));				
				dt.Columns.Add("address1",typeof(string));
				dt.Columns.Add("address2",typeof(string));
				dt.Columns.Add("city",typeof(string));
				dt.Columns.Add("state",typeof(string));
				dt.Columns.Add("country",typeof(string));
				dt.Columns.Add("postalcode",typeof(string));
			}

			foreach(Field fld in fields)
			{
				if (fld.FieldTypeID != 8)
					dt.Columns.Add(fld.Title,typeof(string));
			}

			// retreive responders
			ArrayList responders = GetResponders(f);
			foreach(Responder r in responders) 
			{
				DataRow dr = dt.NewRow();
				if (hasResponder) 
				{
					dr["email"] = r.Email;
					dr["firstname"] = r.Firstname;
					dr["lastname"] = r.Lastname;
					dr["address1"] = r.Address1;
					dr["address2"] = r.Address2;
					dr["city"] = r.City;
					dr["state"] = r.State;
					dr["country"] = r.Country;
					dr["postalcode"] = r.Postalcode;
				}
				foreach(Response res in r.Responses)
				{
					if (res.FieldObject.FieldTypeID != 8)
						dr[res.FieldObject.Title] = res.Value;
				}
				dt.Rows.Add(dr);
			}
			return dt;
		}



		#endregion
	}

	public struct FieldTypeInfo
	{
		public int ID;
		public string Name;
		public string EditSrc;
		public string DisplaySrc;
		public FieldTypeInfo(int id, string name, string editsrc, string displaysrc)
		{
			ID = id;
			Name = name;
			EditSrc = editsrc;
			DisplaySrc = displaysrc;
		}
	}


}
