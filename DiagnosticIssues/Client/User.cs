namespace DiagnosticIssues.Client
{
    public class User
    {
        private Octokit.User _user;

        internal User(Octokit.User user)
        {
            _user = user;
        }

        public string Name => _user?.Login;
    }
}