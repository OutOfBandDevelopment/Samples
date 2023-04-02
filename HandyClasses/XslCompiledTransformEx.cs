using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml.Serialization;

namespace OobDev.InfrastructureSurvey.Common.Xml.Xsl
{
    public static class XslCompiledTransformEx
    {
        public static string Transform(XElement xmlStylesheet, XElement xmlDocument, params XElement[] arguments)
        {
            Contract.Requires(xmlStylesheet != null);
            Contract.Requires(xmlDocument != null);
            Contract.Requires(arguments != null);
            return XslCompiledTransformEx.Transform(xmlStylesheet, xmlDocument, arguments.AsEnumerable());
        }
        public static string Transform(XElement xmlStylesheet, XElement xmlDocument, IEnumerable<XElement> arguments)
        {
            Contract.Requires(xmlStylesheet != null);
            Contract.Requires(xmlDocument != null);
            Contract.Requires(arguments != null);

            var query = arguments.Select(x => new KeyValuePair<XName, XElement>(x.Name, x));
            return XslCompiledTransformEx.Transform(xmlStylesheet, xmlDocument, query);
        }

        public static string Transform(XElement xmlStylesheet, XElement xmlDocument, params KeyValuePair<XName, XElement>[] arguments)
        {
            Contract.Requires(xmlStylesheet != null);
            Contract.Requires(xmlDocument != null);
            Contract.Requires(arguments != null);
            return XslCompiledTransformEx.Transform(xmlStylesheet, xmlDocument, arguments.AsEnumerable());
        }
        public static string Transform(XElement xmlStylesheet, XElement xmlDocument, IEnumerable<KeyValuePair<XName, XElement>> arguments)
        {
            Contract.Requires(xmlStylesheet != null);
            Contract.Requires(xmlDocument != null);
            Contract.Requires(arguments != null);

            var xsltArgumentList = new XsltArgumentList();

            foreach (var argument in arguments)
            {
                var navigator = argument.Value.CreateNavigator();
                xsltArgumentList.AddParam(argument.Key.LocalName, argument.Key.NamespaceName, navigator);
            }

            var transform = new XslCompiledTransform(false);

            using (var stylesheetReader = xmlStylesheet.CreateReader())
            using (var xmlDocumentReader = xmlDocument.CreateReader())
            {
                transform.Load(stylesheetReader);

                using (var outStream = new MemoryStream())
                using (var writer = new StreamWriter(outStream))
                {
                    transform.Transform(xmlDocumentReader, xsltArgumentList, writer);
                    var result = Encoding.UTF8.GetString(outStream.ToArray());
                    return result;
                }
            }
        }

        public static string Transform(string xmlStylesheetPath, string xmlDocumentPath, params XElement[] arguments)
        {
            Contract.Requires(string.IsNullOrEmpty(xmlStylesheetPath));
            Contract.Requires(string.IsNullOrEmpty(xmlDocumentPath));
            Contract.Requires(arguments != null);

            return XslCompiledTransformEx.Transform(xmlStylesheetPath, xmlDocumentPath, arguments.OfType<object>());
        }


        public static string Transform(string xmlStylesheetPath, string xmlDocumentPath, params object[] arguments)
        {
            Contract.Requires(string.IsNullOrEmpty(xmlStylesheetPath));
            Contract.Requires(string.IsNullOrEmpty(xmlDocumentPath));
            Contract.Requires(arguments != null);

            return XslCompiledTransformEx.Transform(xmlStylesheetPath, xmlDocumentPath, arguments.AsEnumerable());
        }

        public static string Transform(string xmlStylesheetPath, string xmlDocumentPath, IEnumerable<object> arguments)
        {
            Contract.Requires(string.IsNullOrEmpty(xmlStylesheetPath));
            Contract.Requires(string.IsNullOrEmpty(xmlDocumentPath));
            Contract.Requires(arguments != null);

            var xsltArgumentList = new XsltArgumentList();

            foreach (var argument in arguments.Where(a => a != null))
            {
                var element = (argument is XDocument) ? (argument as XDocument).Root : (argument as XElement);
                if (element != null)
                {
                    var navigator = element.CreateNavigator();
                    xsltArgumentList.AddParam(element.Name.LocalName, element.Name.NamespaceName, navigator);
                }
                else if (argument is KeyValuePair<string, object>)
                {
                    var item = (KeyValuePair<string, object>)argument;
                    xsltArgumentList.AddExtensionObject(item.Key, item.Value);
                }
                else
                {
                    var type = argument.GetType();

                    string namespaceUri = null;
                    var attribute = type.GetCustomAttributes(typeof(XmlRootAttribute), false).OfType<XmlRootAttribute>().FirstOrDefault();
                    if (attribute != null)
                        namespaceUri = attribute.Namespace;
                    if (string.IsNullOrWhiteSpace(namespaceUri))
                        namespaceUri = "clr-type:" + string.Join(",", type.AssemblyQualifiedName.Split(',').Take(2));


                    xsltArgumentList.AddExtensionObject(namespaceUri, argument);
                }
            }

            var transform = new XslCompiledTransform(true);
            transform.Load(xmlStylesheetPath);

            using (var outStream = new MemoryStream())
            using (var writer = new StreamWriter(outStream))
            {
                transform.Transform(xmlDocumentPath, xsltArgumentList, writer);
                var result = Encoding.UTF8.GetString(outStream.ToArray());
                return result;
            }
        }
    }
}
