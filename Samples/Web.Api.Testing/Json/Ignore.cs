﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Synergy.Web.Api.Testing.Json
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
            if (nodes.Any() == false)
                ignore.Append(new[] {"$.response.body"});
            ignore.Append(nodes.Select(node=> $"$.response.body.{node}"));
            return ignore;
        }

        public static Ignore ResponseLocationHeader()
        {
            return new Ignore("$.response.headers.Location");
        }

        public static Ignore ResponseContentLength()
        {
            return new Ignore("$.response.headers.Content-Length");
        }
    }
}