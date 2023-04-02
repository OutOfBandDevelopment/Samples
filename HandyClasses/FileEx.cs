using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;

namespace ToolKit
{
    public static class FileEx
    {
        public static IEnumerable<string> ReadAsLines(this string filename)
        {
            Contract.Requires(!string.IsNullOrEmpty(filename));
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

            if (!File.Exists(filename))
                yield break;

            using (var reader = new StreamReader(filename))
                while (!reader.EndOfStream)
                    yield return reader.ReadLine();
        }

        public static void WriteAsLinesToFile(this IEnumerable<string> lines, string filename)
        {
            Contract.Requires(lines != null);
            Contract.Requires(!string.IsNullOrEmpty(filename));

            using (var writer = new StreamWriter(filename))
                foreach (var line in lines)
                    writer.WriteLine(line);
        }
    }
}
