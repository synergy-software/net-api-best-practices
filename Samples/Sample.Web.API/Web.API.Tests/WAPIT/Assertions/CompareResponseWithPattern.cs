using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Synergy.Contracts;

namespace Synergy.Samples.Web.API.Tests.WAPIT.Patterns
{
    public class CompareResponseWithPattern : IAssertion
    {
        private readonly string _patternFilePath;
        private readonly Ignore? _ignore;
        private readonly JObject? _savedPattern;

        public CompareResponseWithPattern(string patternFilePath, Ignore? ignore = null)
        {
            _patternFilePath = patternFilePath;
            _ignore = ignore;
            if (File.Exists(patternFilePath))
            {
                var content = File.ReadAllText(patternFilePath);
                _savedPattern = JObject.Parse(content);
            }
        }

        public void Assert(HttpOperation operation)
        {
            var current = operation.Response.Content.ReadJson().OrFail("response");
            if (_savedPattern == null)
            {
                SaveNewPattern(current);
                return;
            }

            JsonComparer patterns = new JsonComparer(_savedPattern, current, _ignore);

            if (operation.TestServer.Repair && patterns.AreEquivalent == false)
            {
                SaveNewPattern(current);
                return;
            }

            Fail.IfFalse(patterns.AreEquivalent,
                         Violation.Of($"Response is different than expected. Verify the differences:\n\n {patterns.GetDifferences()}")
                        );
        }

        private void SaveNewPattern(JToken current)
        {
            File.WriteAllText(_patternFilePath, current.ToString(Formatting.Indented));
        }
    }
}