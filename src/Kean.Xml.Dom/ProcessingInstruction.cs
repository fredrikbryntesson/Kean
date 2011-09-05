using System;
using Kean.Core.Extension;

namespace Kean.Xml.Dom
{
	public class ProcessingInstruction :
		Node
	{
		public string Target { get; set; }
		public string Value { get; set; }
		public ProcessingInstruction() { }
		public ProcessingInstruction(string target) :
			this()
		{
			this.Target = target;
		}
		public ProcessingInstruction(string target, string value) :
			this(target)
		{
			this.Value = value;
		}
        #region Object Overrides
        public override bool Equals(object other)
        {
            return this.Equals(other as ProcessingInstruction);
        }
        public override int GetHashCode()
        {
            return this.Target.Hash() ^ this.Value.Hash();
        }
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
        #region IEquatable<Text> Members
        public bool Equals(ProcessingInstruction other)
        {
            return other.NotNull() &&
                this.Target == other.Target && 
                this.Value == other.Value;
        }
        #endregion
        #region Operators
        public static bool operator ==(ProcessingInstruction left, ProcessingInstruction right)
        {
            return left.Same(right) || left.NotNull() && left.Equals(right);
        }
        public static bool operator !=(ProcessingInstruction left, ProcessingInstruction right)
        {
            return !(left == right);
        }
        #endregion
	}
}
