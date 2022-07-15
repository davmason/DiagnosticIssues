using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticIssues.Client
{
    public class Repository
    {
        private Octokit.Repository _repository;

        public  Repository(string owner, string name)
        {
            _repository = AsyncHelpers.RunSync(Helpers.GetClient().Repository.Get(owner, name));
        }

        // If you add multiple labels to the octokit request it will only return issues with all labels (& not |)
        private IEnumerable<Issue> GetIssuesForSingleLabel(string label,
                                                           DateTime startDate,
                                                           bool includeClosed)
        {
            Octokit.ItemStateFilter state = includeClosed ? Octokit.ItemStateFilter.All : Octokit.ItemStateFilter.Open;
            Octokit.RepositoryIssueRequest issueRequest = new Octokit.RepositoryIssueRequest()
            {
                Filter = Octokit.IssueFilter.All,
                State = state
            };

            issueRequest.Labels.Add(label);

            IReadOnlyList<Octokit.Issue> issues = AsyncHelpers.RunSync(
                Helpers.GetClient().Issue.GetAllForRepository(_repository.Id, issueRequest));
            return issues.Select(x => new Issue(x));
        }

        public IEnumerable<Issue> GetIssues(HashSet<string> includeLabels,
                                            DateTime startDate,
                                            string milestone = null,
                                            HashSet<string> excludeLabels = null,
                                            bool includeClosed = false)
        {
            List<Issue> issues = new List<Issue>();
            foreach (string label in includeLabels)
            {
                IEnumerable<Issue> thisLabel = GetIssuesForSingleLabel(label, startDate, includeClosed);
                issues.AddRange(thisLabel);
            }

            return issues.Distinct()
                .Where(x => excludeLabels == null || !x.Labels.Any(x => excludeLabels.Contains(x)))
                .Where(x => milestone == null || x.Milestone == milestone);
        }
    }
}
