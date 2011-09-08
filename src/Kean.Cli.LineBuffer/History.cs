using System;
using Kean.Core;
using Kean.Core.Extension;
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
        public Action<string> Writer { get; set; }
        public History(Action<string> writer)
        {
            this.Writer = writer;
            this.buffers.Add(new Buffer(this.Writer));
        }
        public void Add()
        {
            this.buffers.Add(new Buffer(this.Writer));
            this.position = this.buffers.Count - 1;
        }
        public void Save()
        {
            this.Current.MoveCursorEnd();
            this.position = this.buffers.Count - 1;
        }
        public void Previous()
        {
            this.Current.RemoveAndNotDelete();
            this.position--;
            if (this.position < 0)
                this.position = 0;
            this.Current.Write();
        }
        public void Next()
        {
            this.Current.RemoveAndNotDelete();
            this.position++;
            if (this.position >= this.buffers.Count)
                this.position = this.buffers.Count - 1;
            this.Current.Write();
        }
        public void Clear()
        {
            this.buffers = new Kean.Core.Collection.List<Buffer>();
        }
    }
}
