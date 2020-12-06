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
                    this.LoadPage($"https://raw.githubusercontent.com/wiki/{value}/Home.md");
                }
            }
        }


        private async Task LoadPage(string url) {
             var httpClient = new HttpClient();

            var response =  await httpClient.GetAsync(url);
            var contents = await response.Content.ReadAsStringAsync();

            this.MDViewer.Markdown = contents;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}