using System;
using System.Collections;
using System.Data;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Base organizational object for Dury.SiteFoundry.  Includes hierarchy organization, multilingual labels, child collections, html and xml output
	/// </summary>
	public class Node
	{

		#region properties

		private int id;
		private int parentID;
		private string filename;
		private int typeID;
		private bool publish;
		private bool visible;
		private bool visibleMenu;
		private bool visibleSubMenu;
		private int rank;
		private DateTime dateCreated;
		private DateTime dateModified;
		private IList advancedArticleNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList linkNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList nodeNameNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList securityNodeNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList simpleArticleNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList formNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList

		private IList names;

		public int Id
		{
			get { return this.id; }
		}

		public int ParentID
		{
			get { return this.parentID; }
			set { this.parentID = value; }
		}

		public string Filename
		{
			get { return this.filename; }
			set { this.filename = value; }
		}

		public int TypeID
		{
			get { return this.typeID; }
			set { this.typeID = value; }
		}

		public bool Publish
		{
			get { return this.publish; }
			set { this.publish = value; }
		}

		public bool Visible
		{
			get { return this.visible; }
			set { this.visible = value; }
		}

		public bool VisibleMenu
		{
			get { return this.visibleMenu; }
			set { this.visibleMenu = value; }
		}

		public bool VisibleSubMenu
		{
			get { return this.visibleSubMenu; }
			set { this.visibleSubMenu = value; }
		}

		public int Rank
		{
			get { return this.rank; }
			set { this.rank = value; }
		}

		public DateTime DateCreated
		{
			get { return this.dateCreated; }
			set { this.dateCreated = value; }
		}

		public DateTime DateModified
		{
			get { return this.dateModified; }
			set { this.dateModified = value; }
		}

		public IList AdvancedArticleNodeIDs
		{
			get { return this.advancedArticleNodeIDs; }
		}

		public IList LinkNodeIDs
		{
			get { return this.linkNodeIDs; }
		}

		public IList NodeNameNodeIDs
		{
			get { return this.nodeNameNodeIDs; }
		}

		public IList SecurityNodeNodeIDs
		{
			get { return this.securityNodeNodeIDs; }
		}

		public IList SimpleArticleNodeIDs
		{
			get { return this.simpleArticleNodeIDs; }
		}
		public IList FormNodeIDs
		{
			get { return this.formNodeIDs; }
			set { this.formNodeIDs = value; }
		}


		private int pageTemplateID;
		public int PageTemplateID 
		{
			get { return pageTemplateID; }
			set { pageTemplateID = value; }
		}

		public NodePageTemplate Template
		{
			get 
			{
				int templateID = this.pageTemplateID;
				Node current = this;
				while(current != null) 
				{
					if (current.pageTemplateID > -1) 
					{
						templateID = current.pageTemplateID;
						break;
					}
					current = current.parent;
				}
				return Dury.SiteFoundry.PageTemplates.GetPageTemplate(templateID);
			}
		}


		public string URL
		{
			get 
			{
				Node current = this.parent;
				string output = "";
				if (current != null) 
				{
					output = this.filename;
					while (current != null && current.parent != null) 
					{
						output = current.filename + '/' + output;
						current = current.parent;
					}
					if (SFGlobal.ShowLangInUrl)
						output = SFGlobal.CurrentCulture.Name.ToLower() + "/" + output;

					output = "/" + SFGlobal.VirtualDirectory + output;
					if (SFGlobal.VirutalFileExtention.Length > 0)
						output += SFGlobal.VirutalFileExtention;
					else
						output += "/";
				}
				else
				{
					output = SFGlobal.SiteRoot;
					//output = "/";
				}
				return output;
			}
		}

		public System.Collections.IList Names
		{
			get { return names; }
		}

		


		public Node parent;
		public NodeCollection children;// = new NodeCollection();

		#endregion

		#region constructors

		public Node()
		{
		}

		public Node(int newid)
		{
			this.id = newid;
		}

		public Node(Node newparent)
		{
			this.parent = newparent;
			if (this.parent != null)
				this.parentID = this.parent.id;
			else
				this.parentID = 0;
		}


		public Node(Node newparent,string newfilename,int newTypeID)
		{
			this.parent = newparent;
			if (this.parent != null)
				this.parentID = this.parent.id;
			else
				this.parentID = 0;
			this.filename = newfilename;
			this.typeID = newTypeID;
		}


		#endregion

		#region methods

		private int checkRole(string role)
		{
			int i = -1;

			foreach(SecurityNode sn in this.GetRoles())
			{
				if (sn.RoleIDObject.Name == role) 
				{
					i = sn.PermissionLevel;
					break;
				}
			}

			/*
			foreach(NodeRole nr in this.roles)
			{
				if (nr.Name == role)
				{
					//System.Web.HttpContext.Current.Response.Write(this.filename + "=" + nr.Name);
					i = nr.Level;
					break;
				}
			}
			*/
			return i;
		}

		/// <summary>
		/// returns integer representing requested role's permission for the current node.  if permission
		/// isn't found locally, then looks up to the parent to find a role entry matching the requested role.
		/// returns 0 (no permissions) if not found.
		/// </summary>
		/// <param name="role">name of role to check</param>
		/// <returns>a number representing the permission (0-31)</returns>
		public int getRolePermission(string role)
		{
			int i = -1;
            //string s = "nodes:";
			Node p = this;
			while(p != null && i == -1)
			{
				i = p.checkRole(role);
				/*
				if (p.roles != null && p.roles.Count > 0)
				{
					s += "[ name=" + p.filename+" count=" + p.roles.Count.ToString();
                    s += " rolePerm=" + i.ToString();
					s += "], <br>";
				}
				*/	
				
				p = p.parent;
			}
			if (i==-1) i=0;
			//System.Web.HttpContext.Current.Response.Write(s);
			return i;
		}

		/// <summary>
		/// Get a iterable list of roles that this node belongs to.
		/// </summary>
		/// <returns></returns>
		public ArrayList GetRoles()
		{
			System.Web.HttpContext c = System.Web.HttpContext.Current;
			ArrayList al = new ArrayList();
			Node p = this;
			do 
			{
				foreach(SecurityNode sn in p.securityNodeNodeIDs)
				{
					al.Add(sn);
				}
				p = p.parent;
			} while (p!= null);
			return al;
		}


		/// <summary>
		/// Finds the index of this node in it's parents node collection.
		/// </summary>
		/// <returns>integer greater than 0, if parent is null returns -1</returns>
		public int GetIndexInParentCollection() 
		{
			if (this.parent != null) 
			{
				for(int i=0; i<this.parent.children.Count;i++) 
				{
					if (this.parent.children[i] == this)
						return i;
				}
			}
			return -1;
		}

		public bool IsParentOf(Node possibleChild) 
		{
			while (possibleChild != null && possibleChild.parent != null) 
			{
				if (possibleChild == this)
					return true;
				possibleChild = possibleChild.parent;
			}
			return false;
		}

		/// <summary>
		/// get the node label in the supplied language identifier
		/// </summary>
		/// <param name="key">5(xx-XX) character localization string.  eg. "en-CA"</param>
		/// <returns>a string</returns>
		public string getName(string key)
		{
			foreach (NodeName nn in this.NodeNameNodeIDs)
			{
				if (nn.Lang == key)
					return nn.Name;
			}
			//throw new Exception("this node doesn't have a node name defined for this language: " + key);
			return "no label defined";
		}

		/// <summary>
		/// Finds and returns a Node, using the supplied ArrayList as a path to the node.
		/// </summary>
		/// <param name="nodeNames">arraylist of nodenames to use as a path to find a node</param>
		/// <returns>a node</returns>
		public Node Find(System.Collections.ArrayList nodeNames)
		{
			if (nodeNames.Count == 0) return this;
			if (nodeNames.Count == 1) if (nodeNames[0].ToString() == this.filename) return this;
			foreach(Node n in this.children)
			{
				if (n.filename == nodeNames[0].ToString())
				{
					nodeNames.RemoveAt(0);
					return n.Find(nodeNames);
				}
			}
			// error message
			string p = string.Empty;
			foreach(string s in nodeNames)
				p += s + ",";
			p = p.Substring(0,p.Length-1);
			throw new DuryTools.ErrorHandler("Can't Find Node: " + p);
		}

		/// <summary>
		/// Finds and returns a Node, using the supplied ID
		/// </summary>
		/// <param name="nodeID">unique indenity of sought node</param>
		/// <returns>node reference from the root</returns>
		public Node Find(int nodeID) 
		{
			if (this.id == nodeID) return this;
			foreach(Node n in this.children)
			{
				Node n1 = n.Find(nodeID);
				if (n1 != null)
					return n1;
			}
			return null;
		}


		/// <summary>
		/// Finds a node based on it's name
		/// </summary>
		/// <param name="nodeName"></param>
		/// <returns></returns>
		public Node Find(string nodeName) 
		{
			if (this.Filename == nodeName) return this;
			foreach(Node n in this.children)
			{
				Node n1 = n.Find(nodeName);
				if (n1 != null)
					return n1;
			}
			return null;
		}





		public string ToString(int indent)
		{
			string s = "";
			string p = "";
			for(int i=0;i<indent;i++) p+= "-";
			s += p +  this.Filename + "<br>" + System.Environment.NewLine;

			foreach (Node n in this.children)
			{
				s += n.ToString(indent+1);
			}
			return s;
		}

		public string ToPublicXML(string lang)
		{

			string s = "";
			if (this.visible) 
			{
				s += "<menu id=\"" + this.id.ToString() + "\" label=\"" + formatString(this.getName(lang)) + "\" href=\"" + this.URL + "\" >";
				foreach (Node n in this.children)
				{
					s += n.ToPublicXML(lang);
				}
				s += "</menu>";
			}
			return s;
		}

		public string ToXML()
		{
			string s = "";
			s += "<menu id=\"" + this.id + "\" url=\"" + this.URL + "\" label=\"" + formatString(this.getName(SFGlobal.DefaultLanguage)) + "\" type=\"" + this.typeID.ToString() + "\" >";
			foreach (Node n in this.children)
			{
				s += n.ToXML();
			}
			s += "</menu>";
			return s;
		}

		public string ToSelfXML(int depth)
		{
			string s = "";
			s += "<menu id=\"" + this.id + "\" url=\"" + this.URL + "\" label=\"" + formatString(this.getName(SFGlobal.DefaultLanguage)) + "\" type=\"" + this.typeID.ToString() + "\" hasChildren=\"" + (this.children.Count > 0).ToString() + "\" >";
			if (depth > 0) 
			{
				foreach (Node n in this.children)
				{
					int i = depth - 1;
					s += n.ToSelfXML(i);
				}
			}
			s += "</menu>";
			return s;
		}

		private string formatString(string input)
		{
			string[] r = new string[] { "&","&amp;", "'","&apos;", "\"","&quot;", "<","&lt;", ">","&gt;" };
			for(int i=0; i< r.Length; i+=2)
			{
				if (input.IndexOf(r[i]) > 0)
					input = input.Replace(r[i],r[i+1]);
			}
			return input;
		}

		public string ToHTML(int indent)
		{
			string s = "";
			string p = "";
			for(int i=0;i<indent;i++) p+= "&nbsp;&nbsp;&nbsp;";
			s += p +  "<a href=\"" + this.URL + "\">";
			if (System.Web.HttpContext.Current.Request.RawUrl == this.URL) s+= "<b>";
			s +=this.Filename;
			s+= "</a>";
			if (System.Web.HttpContext.Current.Request.RawUrl == this.URL) s+= "</b>";
			s += "<br>" + System.Environment.NewLine;

			foreach (Node n in this.children)
			{
				s += n.ToHTML(indent+1);
			}
			return s;
		}

		public string ToAdminHTML(int indent)
		{
			string s = "";
			string p = "";
			for(int i=0;i<indent;i++) p+= "&nbsp;&nbsp;&nbsp;";
			s += p +  "<a href=\"?nodeID=" + this.id + "\">";
			s +=this.Filename;
			s += "</a>";
			s += "&nbsp;&nbsp;&nbsp;";
			s += "<a href=\"#\" onClick=\"editNode(" + this.id + ");\">edit</a> | <a href=\"#\" onClick=\"deleteNode(" + this.id + ");\">delete</a>";
			s += "<br>" + System.Environment.NewLine;

			foreach (Node n in this.children)
			{
				s += n.ToAdminHTML(indent+1);
			}
			return s;
		}

		public ArrayList GetSimpleChildren()
		{
			ArrayList al = new ArrayList();
			foreach(Node n in this.children)
			{
				al.Add(n.GetSimple(0));
			}
			return al;
		}

		public NodeJSON GetSimple(int depth)
		{
			bool ucm = SFGlobal.CurrentUser.CanUserModify(this);
			NodeJSON nss = new NodeJSON(this.Id,this.URL,this.typeID,this.getName(SFGlobal.DefaultLanguage),(this.children.Count>0) ? true : false,ucm);
			if (depth > 0) 
			{
				foreach (Node n in this.children)
				{
					int i = depth - 1;
					nss.children.Add(n.GetSimple(i));
				}
			}
			return nss;
		}
		#endregion

	}

	public class NodeJSON
	{
		public int id;
		public string url;
		public int type;
		public string label;
		public bool hasChildren;
		public bool userCanModify;
		public ArrayList children;

		public NodeJSON(int Id, string Url, int Type, string Label, bool HasChildren, bool UserCanModify)
		{
			id = Id;
			url = Url;
			type = Type;
			label = Label;
			hasChildren = HasChildren;
			userCanModify = UserCanModify;
			children = new ArrayList();
		}
	}



}
