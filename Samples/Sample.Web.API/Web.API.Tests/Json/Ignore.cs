﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Synergy.Samples.Web.API.Extensions;

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

        public static Ignore ResponseBody(params string[] nodes)
        {
            var ignore = new Ignore();
            if (nodes.IsEmpty())
                ignore.Append(new[] {"$.response.body"});
            ignore.Append(nodes.Select(node=> $"$.response.body.{node}"));
            return ignore;
        }
    }
}