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
        public JToken Pattern { get; }
        public JToken New { get; }
        public Ignore Ignore { get; }

        public bool AreEquivalent => JToken.DeepEquals(Pattern, toCompare);

        public JsonComparer(JToken pattern, JToken @new, Ignore? ignore = null)
        {
            Pattern = pattern;
            New = @new;
            Ignore = ignore ?? new Ignore();
            this.toCompare = GetJsonToCompareWithIgnoredLines();
        }

        private JToken GetJsonToCompareWithIgnoredLines()
        {
            var copy = New.DeepClone();
            foreach (var line in Ignore.Nodes)
            {
                var patternNodes = Pattern.SelectTokens(line);
                var newNodes = copy.SelectTokens(line);

                foreach (var (newNode, patternNode) in newNodes.Zip(patternNodes, (one, two) => (one, two)))
                {
                    newNode.Replace(patternNode);
                }
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
                    break;
                }

                if (lineNumber >= patternLines.Length)
                {
                    sb.AppendLine($"Line {lineNumber}:");
                    sb.AppendLine($"\tCurrent JSON is longer than expected");
                    break;
                }

                var patternLine = patternLines[lineNumber].Trim();
                var actualLine = newLines[lineNumber].Trim();
                if (patternLine != actualLine)
                {
                    sb.AppendLine($"Line {lineNumber}:");
                    sb.AppendLine($"\tExpected: {patternLine}");
                    sb.AppendLine($"\tBut was : {actualLine}");
                }
            }

            return sb.ToString();
        }
    }
}