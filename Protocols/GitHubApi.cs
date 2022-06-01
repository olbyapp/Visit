using Octokit;

namespace Visit.Protocols
{
    public class GitHubApi
    {

        private GitHubClient _client;

        public GitHubApi()
        {
            var client = new GitHubClient(new ProductHeaderValue("my-cool-app"));

            var tokenAuth = new Credentials("token"); // NOTE: not real token
            client.Credentials = tokenAuth;


        }

       
    }
}
