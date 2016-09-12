using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OobDev.Tools.Csv
{
    public static class CsvWriter
    {
        public static async Task WriteAsCsvToAsync<T>(this IEnumerable<T> source, Stream outStream, bool includeHeader = true)
        {
            Contract.Requires(source != null);
            Contract.Requires(outStream != null);

            using (var writer = new StreamWriter(outStream, Encoding.UTF8, 1024 * 32, true))
            {

                var type = typeof(T);
                var properties = (from item in type.GetProperties().Select((p, i) => new { p, i })
                                  let prop = item.p
                                  where prop.CanRead && !(prop.GetIndexParameters()?.Any() ?? false)
                                  let getter = prop.GetGetMethod()
                                  select new
                                  {
                                      Name = item.p.Name,
                                      Index = item.i,
                                      Getter = getter,
                                  }).ToList();

                if (includeHeader)
                    await writer.WriteLineAsync(
                        properties.Aggregate(
                            new StringBuilder(),
                            (sb, prop) => sb.Append($"{(prop.Index > 0 ? "," : null)}\"{prop.Name.Replace(@"""",@"""""").Replace(@"_", @" ")}\""),
                            sb => sb.ToString()
                            )
                        );

                foreach (var item in source.Where(i => i != null))
                    await writer.WriteLineAsync(
                        properties.Aggregate(
                            new StringBuilder(),
                            (sb, prop) => sb.Append($"{(prop.Index > 0 ? "," : null)}\"{prop.Getter.Invoke(item, null)?.ToString().Replace(@"""", @"""""")}\""),
                            sb => sb.ToString()
                            )
                        );
            }
        }

        public static void WriteAsCsvTo<T>(this IEnumerable<T> source, Stream outStream, bool includeHeader = true)
        {
            Contract.Requires(source != null);
            Contract.Requires(outStream != null);

            using (var writer = new StreamWriter(outStream, Encoding.UTF8, 1024 * 32, true))
            {

                var type = typeof(T);
                var properties = (from item in type.GetProperties().Select((p, i) => new { p, i })
                                  let prop = item.p
                                  where prop.CanRead && !(prop.GetIndexParameters()?.Any() ?? false)
                                  let getter = prop.GetGetMethod()
                                  select new
                                  {
                                      Name = item.p.Name,
                                      Index = item.i,
                                      Getter = getter,
                                  }).ToList();

                if (includeHeader)
                    writer.WriteLine(
                        properties.Aggregate(
                            new StringBuilder(),
                            (sb, prop) => sb.Append($"{(prop.Index > 0 ? "," : null)}\"{prop.Name.Replace(@"""", @"""""").Replace(@"_", @" ")}\""),
                            sb => sb.ToString()
                            )
                        );

                foreach (var item in source.Where(i => i != null))
                    writer.WriteLine(
                        properties.Aggregate(
                            new StringBuilder(),
                            (sb, prop) => sb.Append($"{(prop.Index > 0 ? "," : null)}\"{prop.Getter.Invoke(item, null)?.ToString().Replace(@"""", @"""""")}\""),
                            sb => sb.ToString()
                            )
                        );
            }
        }
    }
}
