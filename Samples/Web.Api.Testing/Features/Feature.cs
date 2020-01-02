using System.Collections.Generic;
using JetBrains.Annotations;

namespace Synergy.Samples.Web.API.Tests.WAPIT.Features
{
    public class Feature
    {
        [NotNull]
        public string Title { get; }
        public List<Scenario> Scenarios { get; }

        public Feature([NotNull] string title)
        {
            Title = title;
            Scenarios = new List<Scenario>(10);
        }

        [NotNull]
        public Scenario Scenario([NotNull] string title)
        {
            var scenario = new Scenario(title, Scenarios.Count + 1);
            Scenarios.Add(scenario);
            return scenario;
        }

       

    }
}
