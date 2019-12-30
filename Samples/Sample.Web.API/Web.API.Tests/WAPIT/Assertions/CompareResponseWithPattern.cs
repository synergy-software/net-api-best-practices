using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Synergy.Contracts;

namespace Synergy.Samples.Web.API.Tests.WAPIT.Assertions
{
    public class CompareResponseWithPattern : IAssertion
    {
        private readonly string _patternFilePath;
        private readonly Ignore? _ignore;
        private readonly Mode _mode;
        private readonly JObject? _savedPattern;

        public CompareResponseWithPattern(string patternFilePath, Ignore? ignore = null, Mode mode = Mode.Default)
        {
            _patternFilePath = patternFilePath;
            _ignore = ignore;
            _mode = mode;
            if (File.Exists(patternFilePath))
            {
                var content = File.ReadAllText(patternFilePath);
                _savedPattern = JObject.Parse(content);
            }
        }

        public void Assert(HttpOperation operation)
        {
            // TODO: Add non-nullable annotations to OrFail() - and other contract methods
            var current = operation.Response.Content.ReadJson().OrFail("response")!;
            if (_savedPattern == null)
            {
                SaveNewPattern(current);
                return;
            }

            JsonComparer patterns = new JsonComparer(_savedPattern, current, _ignore);

            if (patterns.AreEquivalent)
                return;

            switch (_mode)
            {
                case Mode.ContractCheck:
                    // Always save new response (contract) when running in contract-check-mode
                    SaveNewPattern(current);
                    break;
                case Mode.Default:
                    if (operation.TestServer.Repair)
                    {
                        SaveNewPattern(current);
                        return;
                    }

                    break;
                default:
                    throw Fail.BecauseEnumOutOfRange(_mode);
            }

            throw Fail.Because(Violation.Of("Response is different than expected. Verify the differences:\n\n {0}", patterns.GetDifferences()));
        }

        private void SaveNewPattern(JToken current)
        {
            File.WriteAllText(_patternFilePath, current.ToString(Formatting.Indented));
        }

        public enum Mode
        {
            ContractCheck = 1,
            Default = 2
        }
    }
}