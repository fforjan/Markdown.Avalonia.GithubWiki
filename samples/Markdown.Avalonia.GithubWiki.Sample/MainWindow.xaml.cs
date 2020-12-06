using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Markdown.Avalonia.GithubWiki.Sample
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}