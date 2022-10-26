using System;
using System.Collections.Generic;

namespace OobDev.Common.Cli
{
    public static class ConsoleEx
    {
        // https://github.com/OutOfBandDevelopment/Samples/blob/master/HandyClasses/ConsoleEx.cs
        public static string? ReadKey(string? prompt = default, string? defaultValue = default, bool show = default, char hideWith = '*')
        {
            Console.Write($"{(string.IsNullOrWhiteSpace(prompt) ? "?" : prompt)} {(show ? defaultValue : new string(hideWith, defaultValue?.Length ?? 0))}");
            ConsoleKeyInfo key;
            var keys = new List<char>(defaultValue ?? string.Empty);
            while (!new[] { ConsoleKey.Enter, ConsoleKey.Escape }.Contains((key = Console.ReadKey(intercept: true)).Key))
            {
                if (new[] { ConsoleKey.Delete, ConsoleKey.Backspace }.Contains(key.Key) && keys.Count > 0)
                {
                    Console.Write("\b \b");
                    keys.RemoveAt(keys.Count - 1);
                }
                else
                {
                    Console.Write(show ? key.KeyChar : hideWith);
                    keys.Add(key.KeyChar);
                }
            }
            Console.WriteLine();
            if (key.Key == ConsoleKey.Escape) return defaultValue ?? string.Empty;
            return new string(keys.ToArray());
        }

        public static string? Prompt(string? prompt = default, string? defaultValue = default)
        {
            if (!string.IsNullOrWhiteSpace(prompt))
                Console.Write("{0} ", prompt);
            if (!string.IsNullOrWhiteSpace(defaultValue))
                Console.Write("{0}", defaultValue);

            var chars = new List<char>(defaultValue ?? "");
            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                    return null;
                else if (key.Key == ConsoleKey.Enter)
                    break;
                else if (key.Key == ConsoleKey.Backspace || key.Key == ConsoleKey.Delete)
                {
                    if (chars.Count > 0)
                    {
                        chars.RemoveAt(chars.Count - 1);
                        Console.Write((char)8);
                        Console.Write(" ");
                        Console.Write((char)8);
                    }
                }
                else
                {
                    chars.Add(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            var result = new string(chars.ToArray());
            return result;
        }

        public static string? PromptSecure(string? prompt = default, string? defaultValue = default, char hideWith = '*')
        {
            if (!string.IsNullOrWhiteSpace(prompt))
                Console.Write("{0} ", prompt);
            if (!string.IsNullOrWhiteSpace(defaultValue))
                Console.Write("{0}", new string(hideWith, defaultValue.Length));

            var chars = new List<char>(defaultValue ?? "");
            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                    return default;
                else if (key.Key == ConsoleKey.Enter)
                    break;
                else if (key.Key == ConsoleKey.Backspace || key.Key == ConsoleKey.Delete)
                {
                    if (chars.Count > 0)
                    {
                        chars.RemoveAt(chars.Count - 1);
                        Console.Write((char)8);
                        Console.Write(" ");
                        Console.Write((char)8);
                    }
                }
                else
                {
                    chars.Add(key.KeyChar);
                    Console.Write(hideWith);
                }
            }
            Console.WriteLine();
            var result = new string(chars.ToArray());
            return result;
        }
    }
}
