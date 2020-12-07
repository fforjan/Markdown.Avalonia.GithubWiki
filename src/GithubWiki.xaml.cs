using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Markdown.Avalonia.Utils;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using MDAv = Markdown.Avalonia;

namespace Markdown.Avalonia.GithubWiki
{
    public class GithubWikiViewer : UserControl, ICommand, IBitmapLoader, ISecurityContext
    {
        private readonly DefaultBitmapLoader defaultBitmapLoader = new DefaultBitmapLoader();

        public static readonly AvaloniaProperty<string> GithubUserNameProperty =
           AvaloniaProperty.RegisterDirect<GithubWikiViewer, string>(
               nameof(GithubUserName),
               o => o.GithubUserName,
               (o, v) => o.GithubUserName = v);

        public static readonly AvaloniaProperty<SecurityMode> SecurityModeProperty =
           AvaloniaProperty.RegisterDirect<GithubWikiViewer, SecurityMode>(
               nameof(SecurityMode),
               o => o.SecurityMode,
               (o, v) => o.SecurityMode = v);

        public static readonly AvaloniaProperty<string> GithubRepositoryProperty =
            AvaloniaProperty.RegisterDirect<GithubWikiViewer, string>(
                nameof(GithubRepository),
                o => o.GithubRepository,
                (o, v) => o.GithubRepository = v);

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

            this.MDViewer.Engine.HyperlinkCommand = this;
            this.MDViewer.Engine.BitmapLoader = this;
        }

        event System.EventHandler ICommand.CanExecuteChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        private string githubUserName;
        public string GithubUserName
        {
            get { return githubUserName; }
            set
            {
                if (SetAndRaise(GithubUserNameProperty, ref githubUserName, value) && ConfigurationIsValid && isAttached)
                {
                    this.LoadPage(GetRootPage());
                }
            }
        }

        private SecurityMode securityMode;
        public SecurityMode SecurityMode
        {
            get { return securityMode; }
            set
            {
                if (SetAndRaise(SecurityModeProperty, ref securityMode, value)&& ConfigurationIsValid && isAttached)
                {
                    this.LoadPage(GetRootPage());
                }
            }
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            this.isAttached=true;
            base.OnAttachedToVisualTree(e);
            if(this.ConfigurationIsValid) {
                this.LoadPage(GetRootPage());
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
             this.isAttached=false;
            base.OnDetachedFromVisualTree(e);
        }

        private string githubRepository;
        public string GithubRepository
        {
            get { return githubRepository; }
            set
            {
                if (SetAndRaise(GithubRepositoryProperty, ref githubRepository, value) && ConfigurationIsValid && isAttached)
                {
                    this.LoadPage(GetRootPage());
                }
            }
        }

        private string rootPage = "Home";
        private bool isAttached;

        public string RootPage
        {
            get { return rootPage; }
            set
            {
                if (SetAndRaise(RootPageProperty, ref rootPage, value) && ConfigurationIsValid && isAttached)
                {
                    this.LoadPage(GetRootPage());
                }
            }
        }

        private bool ConfigurationIsValid
        {
            get
            {
                return !string.IsNullOrEmpty(this.RootPage)
                        && !string.IsNullOrEmpty(this.GithubUserName)
                        && !string.IsNullOrEmpty(this.GithubRepository);
            }
        }

        string IBitmapLoader.AssetPathRoot { set => defaultBitmapLoader.AssetPathRoot = value; }

        private void LoadPage(string url)
        {
            this.MDViewer.Markdown = Task.Run(async () =>
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(url);
                return await response.Content.ReadAsStringAsync();

            }).Result;
        }

        private string GetRootPage() => GetPageUri(this.RootPage);

        private string GetPageUri(string page) => $"https://raw.githubusercontent.com/wiki/{this.GithubUserName}/{this.GithubRepository}/{page}.md";
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            if (Uri.TryCreate((string)parameter, UriKind.RelativeOrAbsolute, out var uri))
            {
                if (uri.IsAbsoluteUri)
                {
                    DefaultHyperlinkCommand.GoTo(uri.AbsoluteUri);
                }
                else
                {
                    this.LoadPage(this.GetPageUri(uri.OriginalString));
                }
            }
        }

        Bitmap IBitmapLoader.Get(string urlTxt)
        {
            if (this.CanDownload(urlTxt))
            {
                return defaultBitmapLoader.Get(urlTxt);
            }

            return null;
        }
    }
}