using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Synergy.Samples.Web.API.Tests.WAPIT.Features
{
    public class Markdown
    {
        private readonly Feature _feature;

        public Markdown(Feature feature)
        {
            _feature = feature;
        }

        public string GenerateReportTo(string? filePath = null)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine($"# {_feature.Title}");
            report.AppendLine();

            InsertTableOfContents(report);

            foreach (var scenario in _feature.Scenarios)
            {
                report.AppendLine();
                report.AppendLine($"## {GetScenarioTitle(scenario)}");
                report.AppendLine();
                InsertScenarioStatusTable(report, scenario);
                report.AppendLine();
                
                foreach (var step in scenario.Steps)
                {
                    report.AppendLine($"### {scenario.No}.{step.No}. {step.Title} ({step.Operations.Count} request{GetPluralSuffix(step.Operations)})");
                    report.AppendLine();

                    foreach (var operation in step.Operations)
                    {
                        report.AppendLine($"### {scenario.No}.{step.No}.{step.No}. {GetOperationRequestTitle(operation)}");
                        report.AppendLine();
                        //report.AppendLine($"<details><summary>Details</summary>");
                        //report.AppendLine();
                        InsertRequest(report, operation);
                        report.AppendLine();
                        InsertResponse(report, operation);
                        report.AppendLine();
                        InsertOperationResponseStatusTable(report, operation);
                        report.AppendLine();
                        //report.AppendLine("</details>");
                    }

                    report.AppendLine();
                }
            }

            var reportText = report.ToString();
            if (filePath != null)
                File.WriteAllText(filePath, reportText);

            return reportText;
        }

        private string GetPluralSuffix<T>(IEnumerable<T> elements)
            => elements.Count() == 1 ? "" : "s";

        private void InsertTableOfContents(StringBuilder report)
        {
            foreach (var scenario in _feature.Scenarios)
            {
                report.AppendLine($"1. [{scenario.Title}](#{GetUrlTo(GetScenarioTitle(scenario))})");
            }
        }

        private string GetScenarioTitle(Scenario scenario) 
            => $"{scenario.No}. {scenario.Title} ({scenario.Steps.Count} step{GetPluralSuffix(scenario.Steps)})";

        private string GetUrlTo(string header)
        {
            var url = header.ToLower().Replace(" ", "-");
            return Regex.Replace(url, "[^a-zA-Z0-9_-]+", "", RegexOptions.Compiled);
        }

        private static void InsertScenarioStatusTable(StringBuilder report, Scenario scenario)
        {
            report.AppendLine("| # | Step Actions | Status |");
            report.AppendLine("| - | - | - |");
            foreach (var step in scenario.Steps)
            {
                report.AppendLine($"| {step.No} | {step.Title} | OK |");
            }
        }

        private static void InsertOperationResponseStatusTable(StringBuilder report, HttpOperation operation)
        {
            report.AppendLine($"| Expected Results  | Status |");
            report.AppendLine($"| - | - |");
            foreach (var assertion in operation.Assertions.Cast<IExpectation>())
            {
                report.AppendLine($"| {assertion.ExpectedResult} | OK |");
            }
        }

        private static void InsertRequest(StringBuilder report, HttpOperation operation)
        {
            // TODO: Read the request details from saved pattern (if exists) instead of operation 
            var request = operation.Request;

            report.AppendLine("- Request");
            report.AppendLine("```");
            report.AppendLine(request.GetRequestedUrl());
            InsertHeaders(report, request.Headers);

            var requestBody = request.Content.ReadJson();
            if (requestBody != null)
            {
                report.AppendLine(requestBody.ToString(Formatting.Indented));
            }

            report.AppendLine("```");
        }

        private static string GetOperationRequestTitle(HttpOperation operation)
        {
            var description = operation.Description;
            if (String.IsNullOrWhiteSpace(description))
                return "Request";

            return $"Request to [{description}]";
        }

        private static void InsertResponse(StringBuilder report, HttpOperation operation)
        {
            // TODO: Read the response details from saved pattern (if exists) instead of operation
            var response = operation.Response;

            report.AppendLine("- Response");
            report.AppendLine("```");
            report.AppendLine($"HTTP/{response.Version} {(int) response.StatusCode} {response.StatusCode}");
            InsertHeaders(report, response.Headers);
            var responseBody = response.Content.ReadJson()!;
            report.AppendLine(responseBody.ToString(Formatting.Indented));
            report.AppendLine("```");
        }

        private static void InsertHeaders(StringBuilder report, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            foreach (var header in headers)
            {
                var value = String.Join(", ", header.Value);
                if (String.IsNullOrWhiteSpace(value))
                    continue;

                report.AppendLine($"{header.Key}: {value}");
            }
        }
    }
}