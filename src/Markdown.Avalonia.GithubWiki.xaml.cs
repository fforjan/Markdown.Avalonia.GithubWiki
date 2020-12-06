using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Markdown.Avalonia.GithubWiki
{
    public class GithubWikiViewer : UserControl
    {
        public GithubWikiViewer()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}