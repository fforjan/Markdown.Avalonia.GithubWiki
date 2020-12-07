using System;

namespace Markdown.Avalonia.GithubWiki
{
    static class SecurityChecker {
        private const string GitHubHostName = "github.com";
        private const string GitHubUserContent = "githubusercontent.com";

        public static bool CanDownload(this ISecurityContext context, string urlTxt) {
             if (!Uri.TryCreate((string)urlTxt, UriKind.Absolute, out var uri)) {
                 return false;
             }

            switch(context.SecurityMode) {
                case SecurityMode.ProjectOnly:
                    return (uri.DnsSafeHost == GitHubHostName || uri.DnsSafeHost == GitHubUserContent)
                        && sameUserNameAndRepository();
                case SecurityMode.SameProjectAndAnyUserContent:
                    return (uri.DnsSafeHost == GitHubHostName && sameUserNameAndRepository()) || uri.DnsSafeHost == GitHubUserContent;
                case SecurityMode.AllProjectsAndAnyUserContent:
                    return uri.DnsSafeHost == GitHubHostName || uri.DnsSafeHost == GitHubUserContent;
                case SecurityMode.Anything:
                    return true;
            }

            return false;

            bool sameUserNameAndRepository() {
                return uri.Segments[1].Trim('/') == context.GithubUserName
                && uri.Segments[2].Trim('/') == context.GithubRepository;
            }
        }

    }

}