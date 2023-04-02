using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceScanner
{
    public static class ResoucesEx
    {
        private static Assembly ResolveAssembly (this Assembly assembly)
        {
            return assembly ?? Assembly.GetAssembly(typeof(ResoucesEx));
        }


        public static IEnumerable<string> XsltResources()
        {
            return ResoucesEx.XsltResources(null);
        }
        public static IEnumerable<string> XsltResources(this Assembly assembly)
        {
            var asm = assembly.ResolveAssembly();
            var resources =  asm.GetManifestResourceNames();
            var query = resources.Where(r => r.EndsWith(".xslt", StringComparison.InvariantCultureIgnoreCase));
            return query;
        }

        public static Stream GetResourceStream(string resourceName)
        {
            return ResoucesEx.GetResourceStream(null, resourceName);
        }
        public static Stream GetResourceStream(this Assembly assembly, string resourceName)
        {
            var asm = assembly.ResolveAssembly();
            var resouce = asm.GetManifestResourceStream(resourceName);
            return resouce;
        }
    }
}
