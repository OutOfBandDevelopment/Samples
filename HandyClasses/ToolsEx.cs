using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OobDev.CompareEm
{
    public static class ToolsEx
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> input)
        {
            return new HashSet<T>(input);
        }
        public static string AsFileHash(this string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            if (!File.Exists(fileName)) return null;

            using (var sha = System.Security.Cryptography.SHA1.Create())
            using (var file = File.OpenRead(fileName))
            {
                var hash = sha.ComputeHash(file);
                var encoded = Convert.ToBase64String(hash);
                return encoded;
            }
        }
        
        public static IEnumerable<int> Primes()
        {
            using (var reader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("ConvertTest.Primes.txt")))
                while (!reader.EndOfStream)
                    yield return int.Parse(reader.ReadLine());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng = null)
        {
            if (rng == null) rng = new Random();
            var elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                var swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }
        
        public static byte[] AddCrc(this byte[] input, int crcLen = 4)
        {
            Contract.Requires(input != null);

            var crc = MakeCrc2bytes(input, crcLen);
            var result = new byte[input.Length + crc.Length];
            Array.Copy(input, 0, result, 0, input.Length);
            Array.Copy(crc, 0, result, input.Length, crc.Length);
            return result;
        }

        private static byte[] MakeCrc2bytes(byte[] x, int crcLen)
        {
            Contract.Requires(x != null);

            var result = new byte[crcLen];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = x.Concat(result.Take(i))
                             .Aggregate((a, b) => (byte)((a / (149 + i)) ^ (b * (113 - i))));
            }

            return result;
        }
    }
}