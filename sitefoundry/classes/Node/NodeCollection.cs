using System;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Strongly-Typed collection of Nodes
	/// </summary>
	public class NodeCollection : System.Collections.CollectionBase
	{

		public NodeCollection()
		{			
		}

		public void AddRange(NodeCollection nc)
		{
			this.InnerList.AddRange(nc);
		}

		public void Add(Node f)
		{
			this.List.Add(f);
		}
		

		public Node this[int index]
		{
			get	{ return (Node)this.List[index]; }
		}
		

		public Node this[string nodeName]
		{
			get
			{
				for (int i=0;i<this.List.Count;i++)
					if (((Node)this.List[i]).Filename == nodeName)
						return (Node)this.List[i];
				throw new Exception("Node Not Found : " + nodeName);
			}
		}

		public Node Find(int id)
		{
			foreach(Node n in this.InnerList)
			{
				Node nn = n.Find(id);
				if (nn == null) continue;
				else return nn;
			}
			return null;
		}



	}

}
