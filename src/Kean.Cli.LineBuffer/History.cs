using System;
using Kean.Core;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
namespace Kean.Cli.LineBuffer
{
    class History
    {
        int position = 0;
        Kean.Core.Collection.IList<Buffer> buffers = new Kean.Core.Collection.List<Buffer>();
        public Buffer Current { get { return this.buffers[this.position]; } }
        public int Count { get { return this.buffers.Count; } }
        public bool Empty { get { return this.buffers.Count == 0; } }
        public History()
        {}
        public void Add(Buffer buffer)
        {
            if (!this.buffers.Exists(b => b == buffer))
            {
                this.buffers.Add(buffer);
                this.position = this.buffers.Index(b => b == buffer);
            }
        }
        public Buffer Previous()
        {
            this.position = (this.buffers.Count + this.position - 1) % this.buffers.Count;
            return this.buffers[this.position];
        }
        public Buffer Next()
        {
            this.position = (this.position + 1) % this.buffers.Count;
            return this.buffers[this.position];
        }
        public void Clear()
        {
            this.buffers = new Kean.Core.Collection.List<Buffer>();
        }
    }
}
