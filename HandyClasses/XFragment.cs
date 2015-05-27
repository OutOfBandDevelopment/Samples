using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace OobDev.Common.Xml.Linq
{
    public class XFragment : IEnumerable<XNode>
    {
        private Lazy<IEnumerable<XNode>> Nodes { get; set; }

        public XFragment(Func<IEnumerable<XNode>> nodeFactory)
        {
            this.Nodes = new Lazy<IEnumerable<XNode>>(nodeFactory ?? (() => Enumerable.Empty<XNode>()), true);
        }

        public XFragment(XNode node, params XNode[] nodes)
            : this(() => new[] { node }.Concat(nodes ?? Enumerable.Empty<XNode>()))
        {
        }

        public XFragment(IEnumerable<XNode> nodes)
            : this(() => nodes)
        {
        }

        public XFragment(string xml)
            : this(() => XFragment.Parser(xml).ToArray())
        {
        }

        private static IEnumerable<XNode> Parser(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                yield break;

            var settings = new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment,
                IgnoreWhitespace = true
            };

            using (var stringReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(stringReader, settings))
            {
                xmlReader.MoveToContent();
                while (xmlReader.ReadState != ReadState.EndOfFile)
                {
                    yield return XNode.ReadFrom(xmlReader);
                }
            }
        }

        public override string ToString()
        {
            return this;
        }

        public IEnumerator<XNode> GetEnumerator()
        {
            return (this.Nodes.Value ?? Enumerable.Empty<XNode>()).Where(n => n != null).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        public static XFragment Parse(string xml)
        {
            return new XFragment(xml);
        }

        public static implicit operator XFragment(string xml)
        {
            return new XFragment(xml);
        }
       
        public static implicit operator string(XFragment fragment)
        {
            if (fragment == null)
                return null;

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel= ConformanceLevel.Fragment,
            };
            var sb = new StringBuilder();
            using (var xmlwriter = XmlWriter.Create(sb, settings))
            {
                foreach (var node in fragment)
                {
                    xmlwriter.WriteNode(node.CreateReader(), false);
                }
            }

            return sb.ToString();
        }

        public static implicit operator XFragment(XNode[] nodes)
        {
            return new XFragment(nodes);
        }

        public static implicit operator XFragment(XNode node)
        {
            return new XFragment(node);
        }
    }
    public static class XFragmentEx
    {
        public static XFragment ToXFragment(this IEnumerable<XNode> nodes)
        {
            return new XFragment(nodes);
        }
    }
}
