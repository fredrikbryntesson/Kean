using System;
using Kean.Core;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
namespace Kean.Cli.LineBuffer
{
    class History
    {
        int position = 0;
        Kean.Core.Collection.IList<string> buffers = new Kean.Core.Collection.List<string>();
        public string Current { get { return this.buffers[this.position]; } }
        public int Count { get { return this.buffers.Count; } }
        public bool Empty { get { return this.buffers.Count == 0; } }
        public History()
        { }
        public void Add(string buffer)
        {
            if (!this.buffers.Exists(b => b == buffer))
            {
                this.buffers.Add(buffer);
                this.position = this.buffers.Index(b => b == buffer);
            }
        }
        public string Previous()
        {
            this.position--;
            if (this.position < 0)
                this.position = 0;
            return this.buffers[this.position];
        }
        public string Next()
        {
            this.position++;
            if (this.position >= this.buffers.Count)
                this.position = this.buffers.Count - 1;
            return this.buffers[this.position];
        }
        public void Clear()
        {
            this.buffers = new Kean.Core.Collection.List<string>();
        }
    }
}
