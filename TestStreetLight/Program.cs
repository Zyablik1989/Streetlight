using System;
using System.Collections.Generic;
using System.Text;
using Streetlight;

namespace TestStreetLight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var Streetlight = StreetlightFactory.CreateStreetlight();
                //Console.WriteLine(Streetlight.SecondsLeft);
                var scheme = Streetlight.DigitsSegmentsScheme;
                DrawSegments(scheme);
                Console.ReadKey();
                Console.Clear();
            }
        }

            private static void DrawSegments(List<byte> scheme)
        {
            foreach (var digitScheme in scheme)
            {
                var textScheme = Convert.ToString(digitScheme, toBase: 2).PadLeft(8, '0').ToCharArray();

                var sb = new StringBuilder();
                sb.Append(" ");
                sb.Append((textScheme[0] == '1' ? "—" : " "));
                sb.AppendLine();
                sb.Append((textScheme[1] == '1' ? "|" : " "));
                sb.Append(" ");
                sb.Append((textScheme[2] == '1' ? "|" : " "));
                sb.AppendLine();
                sb.Append(" ");
                sb.Append((textScheme[3] == '1' ? "—" : " "));
                sb.AppendLine();
                sb.Append((textScheme[4] == '1' ? "|" : " "));
                sb.Append(" ");
                sb.Append((textScheme[5] == '1' ? "|" : " "));
                sb.AppendLine();
                sb.Append(" ");
                sb.Append((textScheme[6] == '1' ? "—" : " "));
                Console.WriteLine(sb.ToString());
            }
        }
    }
}
