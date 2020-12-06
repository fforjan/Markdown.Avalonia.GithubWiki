using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Net.Http;
using System.Threading.Tasks;

using MDAv = Markdown.Avalonia;

namespace Markdown.Avalonia.GithubWiki
{
    public class GithubWikiViewer : UserControl
    {
         public static readonly AvaloniaProperty<string> GitHubProjectProperty =
            AvaloniaProperty.RegisterDirect<GithubWikiViewer, string>(
                nameof(GitHubProject),
                o => o.GitHubProject,
                (o, v) => o.GitHubProject = v);

        public static readonly AvaloniaProperty<string> RootPageProperty =
            AvaloniaProperty.RegisterDirect<GithubWikiViewer, string>(
                nameof(RootPage),
                o => o.RootPage,
                (o, v) => o.RootPage = v);

        private MDAv.MarkdownScrollViewer MDViewer;
        

        
        public GithubWikiViewer()
        {
            InitializeComponent();
            this.MDViewer = this.Get<MDAv.MarkdownScrollViewer>("MDViewer");
        }


        private string gitHubProject;
        public string GitHubProject
        {
            get { return gitHubProject; }
            set
            {
                if(SetAndRaise(GitHubProjectProperty, ref gitHubProject, value)) {
                    this.LoadPage(GetRootPage());
                }
            }
        }

        private string rootPage = "Home";
        public string RootPage
        {
            get { return rootPage; }
            set
            {
                if(SetAndRaise(RootPageProperty, ref rootPage, value)) {
                    this.LoadPage(GetRootPage());
                }
            }
        }


        private async Task LoadPage(string url) {
             var httpClient = new HttpClient();

            var response =  await httpClient.GetAsync(url);
            var contents = await response.Content.ReadAsStringAsync();

            this.MDViewer.Markdown = contents;
            this.MDViewer.Engine.HyperlinkCommand = this.NavigateTo;
        }

        private string GetRootPage() => GetPageUri(this.RootPage);

        private string GetPageUri(string page) => $"https://raw.githubusercontent.com/wiki/{this.GitHubProject}/{page}.md";
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void NavigateTo(string page) {

        }
    }
}