using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace ReferenceScanner
{
    public static class XElementEx
    {
        public static XDocument ReadAsXml(this Stream stream)
        {
            Contract.Requires(stream != null);
            Contract.Ensures(Contract.Result<XDocument>() != null);

            var xml = XDocument.Load(stream);
            return xml;
        }

        public static Stream TransformTo(this XContainer container, XContainer styleSheet, bool enableDebug = true)
        {
            Contract.Requires(container != null);
            Contract.Requires(styleSheet != null);

            var xslt = new XslCompiledTransform(enableDebug);

            var results = new MemoryStream();
            using (var containerReader = container.CreateReader())
            using (var styleSheetReader = styleSheet.CreateReader())
            using (var resultsWriter = XmlWriter.Create(results))
            {
                xslt.Load(styleSheetReader);
                xslt.Transform(containerReader, resultsWriter);
            }
            results.Position = 0;

            return results;
        }



        public static T SaveAs<T>(this T node, params string[] filenames) where T : XContainer
        {
            Contract.Requires(filenames != null);

            node.SaveAs(filenames.AsEnumerable());

            return node;
        }

        public static T SaveAs<T>(this T node, IEnumerable<string> filenames) where T : XContainer
        {
            Contract.Requires(filenames != null);

            foreach (var filename in filenames)
                node.SaveAs(filename);

            return node;
        }
        public static T SaveAs<T>(this T node, string filename) where T : XContainer
        {
            Contract.Requires(!string.IsNullOrEmpty(filename));
            Contract.Requires(!string.IsNullOrWhiteSpace(filename));

            if (node == null) return null;

            (node as XElement)?.Save(filename);
            (node as XDocument)?.Save(filename);

            return node;
        }
    }
}
