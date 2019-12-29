using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Synergy.Contracts;
using Synergy.Samples.Web.API.Tests.WAPIT;

namespace Synergy.Samples.Web.API.Tests
{
    public class CompareContractWithPattern : IAssertion
    {
        private readonly string _patternFilePath;
        private readonly Ignore? _ignore;
        private readonly JToken? _savedPattern;

        public CompareContractWithPattern(string patternFilePath, Ignore? ignore = null)
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

            var patterns = new JsonComparer(_savedPattern, current, _ignore);

            if (patterns.AreEquivalent) 
                return;
            
            SaveNewPattern(current);

            throw Fail.Because(Violation.Of("Contract is different than expected. Verify the differences:\n\n {0}", patterns.GetDifferences()));
        }

        private void SaveNewPattern(JToken current)
        {
            File.WriteAllText(_patternFilePath, current.ToString(Formatting.Indented));
        }
    }
}