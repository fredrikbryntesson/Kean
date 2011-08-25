using System;
using Kean.Core.Basis.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;

namespace Kean.Xml.Dom
{
	public class Element:
		Node,
		Collection.IList<Node>
	{
		Collection.IList<Node> childNodes = new Collection.Linked.List<Node>();

		public string Name { get; set; }
		public Collection.IList<Attribute> Attributes { get; private set; }

		public Element()
		{
		}
		protected override void ChangeDocument(Document document)
		{
			base.ChangeDocument(document);
			this.Attributes.Apply(attribute => { attribute.Document = document; });
			this.Apply(node => { node.Document = document; });
		}
		protected override void ChangeParent(Element parent)
		{
			base.ChangeParent(parent);
			this.Attributes.Apply(attribute => { attribute.Parent = parent; });
			this.Apply(node => { node.Parent = parent; });
		}
		#region IList<Node> Members
		public void Add(Node item) 
		{
			item.Parent = this;
			this.childNodes.Add(item); 
		}
		public Node Remove() 
		{ 
			Node result = this.childNodes.Remove();
			result.Parent = null;
			return result;
		}
		public void Insert(int index, Node item) 
		{
			item.Parent = this;
			this.childNodes.Insert(index, item); 
		}
		public Node Remove(int index) 
		{
			Node result = this.childNodes.Remove(index);
			result.Parent = null;
			return result;
		}
		#endregion
		#region IVector<Node> Members
		public int Count { get { return this.childNodes.Count; } }
		public Node this[int index]
		{
			get { return this.childNodes[index]; }
			set 
			{
				this.childNodes[index].Parent = null;
				value.Parent = this;
				this.childNodes[index] = value;
			}
		}
		#endregion
		#region IEnumerable<Node> Members
		public System.Collections.Generic.IEnumerator<Node> GetEnumerator() { return this.childNodes.GetEnumerator(); }
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return (this.childNodes as System.Collections.IEnumerable).GetEnumerator(); }
		#endregion
		#region IEquatable<IVector<Node>> Members
		public bool Equals(Kean.Core.Collection.IVector<Node> other) { return this.childNodes.Equals(other); }
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return base.Equals(other);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public override string ToString()
		{
			return base.ToString();
		}
		#endregion
	}
}
