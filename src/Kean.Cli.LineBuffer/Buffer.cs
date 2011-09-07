using System;
using System.Text;

namespace Kean.Cli.LineBuffer
{
    class Buffer
    {
        public Action<string> Writer { get; set; }
        int cursor = 0;
        StringBuilder line = new StringBuilder();
        public Buffer(Action<string> writer)
        {
            this.Writer = writer;
        }
        public void MoveCursorHome()
        {
            this.MoveCursor(-this.cursor - 1);
        }
        public void MoveCursorLeft()
        {
            if (this.cursor > 0)
            {
                this.MoveCursor(-1);
                this.cursor--;
            }
        }
        public void MoveCursorEnd()
        {
            this.MoveCursor(this.cursor - this.line.Length);
            this.cursor = this.line.Length - 1;
        }
        public void MoveCursorRight()
        {
            if (this.cursor < this.line.Length - 1)
            {
                this.MoveCursor(1);
                this.cursor++;
            }
        }
        public void MoveCursorLeftAndDelete()
        {
            if (this.cursor > 0)
            {
                this.line.Remove(this.line.Length - 1, 1);
                this.cursor--;
                this.Writer((char)8 + " " + (char)8);
            }
        }
        public void Insert(char value)
        {
            this.line.Insert(this.cursor, value);
            this.Writer(this.line.ToString().Substring(this.cursor++)); 
            this.MoveCursor(this.cursor - this.line.Length);
        }
        public void Insert(string value)
        {
            this.line.Insert(this.cursor, value);
            this.Writer(this.line.ToString().Substring(this.cursor));
            this.cursor += value.Length;
            this.MoveCursor(this.cursor - this.line.Length);
        }
        public void Renew(string line)
        {
            this.MoveCursor(-this.line.Length);
            this.cursor = line.Length;
            this.line = new System.Text.StringBuilder(line);
        }
        public void MoveCursor(int steps)
        {
            if (steps < 0)
            {
                char[] buffer = new char[-steps];
                while (steps < 0)
                    buffer[-++steps] = (char)8;
                this.Writer(new String(buffer));
            }
            else if (steps > 0)
                this.Writer(this.line.ToString().Substring(this.cursor, steps));
        }
        public void Write()
        {
            this.Writer(this.line.ToString());
        }
        public void Clear()
        {
            this.cursor = 0;
            this.line = new StringBuilder();
        }
        public void Remove()
        {
            while (this.line.Length > 0)
                this.MoveCursorLeftAndDelete();
            this.cursor = 0;
        }
        public override string ToString()
        {
            return this.line.ToString();
        }
    }
}
