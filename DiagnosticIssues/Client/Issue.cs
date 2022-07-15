
namespace DiagnosticIssues.Client
{
    public class Issue
    {
        private Octokit.Issue _issue;

        internal Issue(Octokit.Issue issue)
        {
            _issue = issue;
        }

        public int Number => _issue.Number;
        
        public string Title => _issue.Title;
        
        public User Assignee => new User(_issue.Assignee);

        public IEnumerable<string> Labels => _issue.Labels.Select(x => x.Name);

        public string Milestone => _issue.Milestone?.Title;

        public DateTimeOffset CreatedAt => _issue.CreatedAt;

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj is not Issue)
            {
                return false;
            }

            Issue other = (Issue)obj;
            return Number == other.Number;
        }

        public override int GetHashCode()
        {
            return Number;
        }

        public override string ToString()
        {
            return $"{Number}: {Title}";
        }
    }
}