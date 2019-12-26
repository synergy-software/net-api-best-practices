using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Synergy.Samples.Web.API.Tests 
{
    public class JsonComparer
    {
        private readonly JToken toCompare;
        public JObject Pattern { get; }
        public JObject New { get; }
        public string[] Ignore { get; }

        public bool AreEquivalent => JToken.DeepEquals(Pattern, toCompare);

        public JsonComparer(JObject pattern, JObject @new, params string[] ignore)
        {
            Pattern = pattern;
            New = @new;
            Ignore = ignore;
            this.toCompare = GetJsonToCompareWithIgnoredLines();
        }

        private JToken GetJsonToCompareWithIgnoredLines()
        {
            var copy = New.DeepClone();
            foreach (var line in Ignore)
            {
                var patternLine = Pattern.SelectToken(line);
                var newLine = copy.SelectToken(line);
                newLine.Replace(patternLine);
            }

            return copy;
        }

        public string? GetDifferences()
        {
            if (AreEquivalent)
                return null;
            
            var sb = new StringBuilder();
            var patternLines = Pattern.ToString(Formatting.Indented).Split(Environment.NewLine);
            var newLines = toCompare.ToString(Formatting.Indented).Split(Environment.NewLine);
            var maxLine = Math.Max(patternLines.Length, newLines.Length);
            for (int lineNumber = 0; lineNumber < maxLine; lineNumber++)
            {
                if (lineNumber >= newLines.Length)
                {
                    sb.AppendLine($"Line {lineNumber}:");
                    sb.AppendLine($"\tCurrent JSON is shorten than expected");
                }
                else if (lineNumber >= patternLines.Length)
                {
                    sb.AppendLine($"Line {lineNumber}:");
                    sb.AppendLine($"\tCurrent JSON is longer than expected");
                }
                else
                {
                    var patternLine = patternLines[lineNumber].Trim();
                    var actualLine = newLines[lineNumber].Trim();
                    if (patternLine != actualLine)
                    {
                        sb.AppendLine($"Line {lineNumber}:");
                        sb.AppendLine($"\tExpected: {patternLine}");
                        sb.AppendLine($"\tBut was : {actualLine}");
                    }
                }
            }

            return sb.ToString();
        }
    }
}