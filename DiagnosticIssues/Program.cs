

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

foreach (string label in includeLabels)
{
    IEnumerable<Issue> current = issues.Where(issue => issue.Labels.Contains(label, StringComparer.InvariantCultureIgnoreCase)).OrderBy(x => x.Number);

    Console.WriteLine($"{label} total count: {current.Count()}");

    IEnumerable<string> uniqueUsernames = current.Select(x => x.Assignee.Name).Distinct();
    Dictionary<string, IEnumerable<Issue>> issuesByUser = new Dictionary<string, IEnumerable<Issue>>();
    foreach (string username in uniqueUsernames)
    {
        IEnumerable<Issue> userIssues;
        string displayName;
        if (username == null)
        {
            displayName = "<Unassigned>";
            userIssues = current.Where(x => x.Assignee?.Name == null);
        }
        else
        {
            displayName = username;
            userIssues = current.Where(x => x.Assignee?.Name?.Equals(username, StringComparison.InvariantCultureIgnoreCase) ?? false);
        }

        issuesByUser[displayName] = userIssues;
    }

    foreach (string username in issuesByUser.Keys.OrderByDescending(x => issuesByUser[x].Count()))
    {
        Console.WriteLine($"    {username} total count: {issuesByUser[username].Count()}");
        foreach (Issue issue in issuesByUser[username])
        {
            Console.WriteLine($"        {issue.CreatedAt.ToString("d")} {issue.Number}: {issue.Title}");
        }
    }
}