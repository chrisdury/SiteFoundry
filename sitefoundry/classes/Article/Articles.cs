using System;
using System.Collections;
using System.Data;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Summary description for Articles.
	/// </summary>
	public class Articles
	{

		private static readonly Articles instance=new Articles();
		public static Articles GetInstance() { return instance; }
		static Articles() {}
		Articles()
		{
		}

		public static SimpleArticle GetSimpleArticle(int id)
		{
			return new SimpleArticle();
		}

		public static AdvancedArticle GetAdvancedArticle()
		{
			return (AdvancedArticle)SFGlobal.ObjectManager.GetObject(typeof(AdvancedArticle));
		}
		public static AdvancedArticle GetAdvancedArticle(int id)
		{
			return GetAdvancedArticle(id,false);
		}
		public static AdvancedArticle GetAdvancedArticle(int id, bool isPublic) 
		{
			ObjectSpace os = GetObjectSpace(isPublic);            
			return (AdvancedArticle)os.GetObject(typeof(AdvancedArticle),id);
		}

		public static void SaveArticle(object article, bool isPublic) 
		{
			Wilson.ORMapper.ObjectSpace os;
			if (isPublic)
				os = SFGlobal.ObjectManagerPublic;
			else
				os = SFGlobal.ObjectManager;

			os.PersistChanges(article);
		}

		public static void PublishAdvancedArticle(int nodeID, string lang, int rank) 
		{
			AdvancedArticle publicArticle;
			int c = SFGlobal.ObjectManagerPublic.GetObjectCount(typeof(AdvancedArticle),"nodeID=" + nodeID + " AND lang = '" + lang + "' AND rank=" + rank);
			if (c > 0)
				publicArticle = (AdvancedArticle)SFGlobal.ObjectManagerPublic.GetObject(new OPathQuery(typeof(AdvancedArticle),"NodeID=" + nodeID + " && Lang='" + lang + "' && Rank=" + rank));
			else
				publicArticle = (AdvancedArticle)SFGlobal.ObjectManagerPublic.GetObject(typeof(AdvancedArticle));

			AdvancedArticle currentArticle = (AdvancedArticle)SFGlobal.ObjectManager.GetObject(new OPathQuery(typeof(AdvancedArticle),"NodeID=" + nodeID + " && Lang='" + lang + "' && Rank=" + rank));
			publicArticle.Rank = currentArticle.Rank;
			publicArticle.Title = currentArticle.Title;
			publicArticle.Body = currentArticle.Body;
			publicArticle.Keyword = currentArticle.Keyword;
			publicArticle.Lang = currentArticle.Lang;
			publicArticle.NodeID = currentArticle.NodeID;
			publicArticle.Summary = currentArticle.Summary;
			publicArticle.TemplateID = currentArticle.TemplateID;
			publicArticle.UserID = currentArticle.UserID;
			publicArticle.DateCreated = currentArticle.DateCreated;
			publicArticle.DateModified= currentArticle.DateModified;

			SFGlobal.ObjectManagerPublic.PersistChanges(publicArticle);
			SFGlobal.ObjectManager.PersistChanges(currentArticle);
		}



		public static AdvancedArticle GetAdvancedArticle(int nodeID, string lang, int page, int version, bool isPublic)
		{
			ObjectSpace os = GetObjectSpace(isPublic);
			OPathQuery oq = new OPathQuery(typeof(AdvancedArticle),"NodeID==" + nodeID + " && Lang=='" + lang + "' && Rank==" + page + " && Version==" + version,"");
			return (AdvancedArticle)os.GetObject(oq);
		}

		public static ArrayList GetAdvancedArticleContainers(int nodeID)
		{
			ArrayList al = new ArrayList();
			string[] langs = GetLanguages();
			foreach(string l in langs) 
			{
				AdvancedArticleContainer aac = GetAdvancedArticleContainerByNodeID(nodeID,l);
				if (aac.Pages.Count > 0) al.Add(aac);
			}
			return al;
		}

		public static AdvancedArticleContainer GetAdvancedArticleContainerByNodeID(int nodeID, string lang)
		{
			ObjectSpace os = GetObjectSpace(false);
            ObjectSet ol = os.GetObjectSet(new OPathQuery(typeof(AdvancedArticle),"NodeID==" + nodeID + " && Lang=='" + lang + "'","Lang,Rank,Version"));
			AdvancedArticlePageCollection aapc = new AdvancedArticlePageCollection();
			foreach(AdvancedArticle aa in ol)
			{
				aapc.Add(aa);
			}
			return new AdvancedArticleContainer(nodeID,lang,aapc);
		}
		private static string[] GetLanguages()
		{
			string s = String.Empty;
			DataSet ds = SFGlobal.DAL.execDataSet("SELECT DISTINCT(lang) FROM AdvancedArticles");
			if (ds.Tables[0].Rows.Count > 0) 
			{
				foreach(DataRow dr in ds.Tables[0].Rows)
					s += dr[0] +",";                
				s = s.Substring(0,s.Length-1);
			}
			return s.Split(',');            
		}

		private static ObjectSpace GetObjectSpace(bool isPublic)
		{
			Wilson.ORMapper.ObjectSpace os;
			if (isPublic)
				os = SFGlobal.ObjectManagerPublic;
			else
				os = SFGlobal.ObjectManager;
			return os;
		}
	}


	public struct AdvancedArticleContainer
	{
		public string Lang;
		public int NodeID;
		public AdvancedArticlePageCollection Pages;

		public AdvancedArticleContainer(int nodeID, string lang, AdvancedArticlePageCollection pages)
		{
			NodeID = nodeID;
			Lang = lang;
			Pages = pages;
		}
	}

	public class AdvancedArticlePageCollection :  System.Collections.CollectionBase
	{
		public AdvancedArticlePageCollection()
		{			
		}

		public void Add(AdvancedArticle a)
		{
			this.List.Add(a);
		}
		public AdvancedArticle this[int index]
		{
			get	{ return (AdvancedArticle)this.List[index]; }
		}

		public AdvancedArticle GetVersion(int page, int version)
		{
			foreach(AdvancedArticle aa in this.List)
				if (aa.Rank == page && aa.Version == version) return aa;
			throw new Exception("version:" + version + " not found");
		}

		public AdvancedArticle GetPage(int page)
		{
			foreach(AdvancedArticle aa in this.List)
				if (aa.Rank == page) return aa;
			throw new Exception("page:" + page + " not found");
		}
	}

}
