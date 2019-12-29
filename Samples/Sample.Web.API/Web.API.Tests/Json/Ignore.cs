using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Synergy.Samples.Web.API.Tests
{
    public class Ignore
    {
        public ReadOnlyCollection<string> Nodes { get; private set; }

        public Ignore(params string[] nodes)
        {
            Nodes = nodes.ToList().AsReadOnly();
        }

        public void Append(IEnumerable<string> ignores)
        {
            var nodes = Nodes.ToList();
            nodes.AddRange(ignores);
            Nodes = nodes.AsReadOnly();
        }
    }
}