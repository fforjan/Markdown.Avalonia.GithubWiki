using System;

namespace Markdown.Avalonia.GithubWiki
{
    static class SecurityChecker {
        private const string GitHubHostName = ".github.com";
        private const string GitHubUserContent = ".githubusercontent.com";

        public static bool CanDownload(this ISecurityContext context, string urlTxt) {
             if (!Uri.TryCreate((string)urlTxt, UriKind.Absolute, out var uri)) {
                 return false;
             }

            switch(context.SecurityMode) {
                case SecurityMode.ProjectOnly:
                    return (uri.DnsSafeHost.EndsWith(GitHubHostName, true, null) && sameUserNameAndRepository());
                case SecurityMode.ProjectOnlyAndUserContent:
                    return (uri.DnsSafeHost.EndsWith(GitHubHostName, true, null) || uri.DnsSafeHost.EndsWith(GitHubUserContent, true, null))
                        && sameUserNameAndRepository();
                case SecurityMode.SameProjectAndAnyUserContent:
                    return (uri.DnsSafeHost.EndsWith(GitHubHostName, true, null) && sameUserNameAndRepository()) || uri.DnsSafeHost.EndsWith(GitHubUserContent, true, null);
                case SecurityMode.AllProjectsAndAnyUserContent:
                    return uri.DnsSafeHost.EndsWith(GitHubHostName, true, null) || uri.DnsSafeHost.EndsWith(GitHubUserContent, true, null);
                case SecurityMode.Anything:
                    return true;
            }

            return false;

            bool sameUserNameAndRepository() {
                return (uri.Segments.Length > 1 && uri.Segments[1].Trim('/') == context.GithubUserName
                && uri.Segments[2].Trim('/') == context.GithubRepository)
                || sameUserNameAndRepositoryInWiki();
            }

            bool sameUserNameAndRepositoryInWiki() {
                return uri.Segments.Length > 2 && uri.Segments[1].Trim('/') == "wiki"  && uri.Segments[2].Trim('/') == context.GithubUserName
                && uri.Segments[3].Trim('/') == context.GithubRepository;
            }
        }

    }

}