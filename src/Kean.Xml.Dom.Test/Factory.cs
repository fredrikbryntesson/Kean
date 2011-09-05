using System;

namespace Kean.Xml.Dom.Test
{
    class Factory
    {
        public Document Open(string name)
        {
            return Document.OpenResource("Data/" + name + ".xml");
        }

        public Document Create(string name)
        {
            Document result = null;
            switch (name)
            {
                case "valid001": 
                    result = new Document() { Root = new Element("empty") };
                    break;
                case "valid002":
                    {
                        result = new Document() { Root = new Element("text") };
                        result.Root.Add(new Text("Text"));
                    }
                    break;
                case "valid003":
                    {
                        result = new Document() { Root = new Element("library") };
                        Element book = new Element("book");
                        Element title = new Element("title");
                        title.Add(new Text("Write with XML"));
                        book.Add(title);
                        result.Root.Add(book);
                    }
                    break;
                case "valid004":
                    {
                        result = new Document() { Root = new Element("library") };
                        Element book = new Element("book");
                        book.Attributes.Add(new Attribute("language", "english"));
                        Element title = new Element("title");
                        title.Attributes.Add(new Attribute("location", "office"));
                        title.Add(new Text("Write with XML"));
                        book.Add(title);
                        result.Root.Add(book);
                    }
                    break;
                case "valid005":
                    {
                        result = new Document() { Root = new Element("library") };
                        Comment comment = new Comment("New Book");
                        result.Root.Add(comment);
                        Element book = new Element("book");
                        book.Add(new Text("Write with XML"));
                        result.Root.Add(book);
                    }
                    break;
                case "valid006":
                    {
                        result = new Document() { Root = new Element("data") };
                        Data data = new Data("Example");
                        result.Root.Add(data);

                    } 
                    break;
                case "valid007":
                    {
                        result = new Document() { Root = new Element("instruction") };
                        ProcessingInstruction instruction = new ProcessingInstruction("target", "Try this!");
                        result.Root.Add(instruction);
                    }
                    break;
            }
            return result;
        }
    }
}
