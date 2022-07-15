

using DiagnosticIssues.Client;

Console.WriteLine("Hello, World!");


Repository repo = new Repository("dotnet", "runtime");

HashSet<string> includeLabels = new HashSet<string>()
{
    "area-diagnostics-coreclr",
    "area-Tracing-coreclr",
    "area-System.Diagnostics",
    "area-System.Diagnostics.Tracing"
};

HashSet<string> excludeLabels = new HashSet<string>()
{
    "enhancement",
    "feature-request",
    "User Story",
    "os-ios",
    "os-android",
    "os-tvos",
    "arch-wasm"
};

IEnumerable<Issue> issues = repo.GetIssues(includeLabels: includeLabels,
                                           startDate: DateTime.Now - TimeSpan.FromDays(7),
                                           milestone: "7.0.0",
                                           excludeLabels: excludeLabels,
                                           includeClosed: false);

Console.WriteLine($"There are {issues.Count()} open issues in dotnet/runtime diagnostics");
foreach (var issue in issues)
{
    Console.WriteLine($"{issue.Title}");
}
