namespace Markdown.Avalonia.GithubWiki
{
    public enum SecurityMode {
        ProjectOnly,
        SameProjectAndAnyUserContent,
        AllProjectsAndAnyUserContent,
        Anything
    }

    public interface ISecurityContext {
        string GithubRepository{get;}

        string GithubUserName {get;}

        SecurityMode SecurityMode {get;}
    }

}