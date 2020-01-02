using System.Collections.Generic;

namespace Synergy.Samples.Web.API.Tests.WAPIT.Features 
{
    public class Step
    {
        public int No { get; }
        public string Title { get; }
        public List<HttpOperation> Operations = new List<HttpOperation>(1);

        public Step(string title, int no)
        {
            Title = title;
            No = no;
        }

        public void Attach(HttpOperation operation)
        {
            Operations.Add(operation);
        }
    }
}