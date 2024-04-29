using Markdown.Avalonia.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Markdown.Avalonia.GithubWiki
{
    class GithubPathResolver : IPathResolver
    {
        private readonly ISecurityContext securityContext;

        public GithubPathResolver(ISecurityContext securityContext)
        {
            this.securityContext = securityContext;
        }

        public string AssetPathRoot { set; private get; }
        public IEnumerable<string> CallerAssemblyNames { set; private get; }

        public async Task<Stream> ResolveImageResource(string relativeOrAbsolutePath)
        {
            if (!securityContext.CanDownload(relativeOrAbsolutePath, out var uri))
            {
                throw new NotImplementedException();
            }

            return await OpenStream(uri);


            throw new NotImplementedException();
        }

        private async Task<Stream> OpenStream(Uri url)
        {
            switch (url.Scheme)
            {
                // case "file":
                //     if (!File.Exists(url.LocalPath)) return null;
                //     return File.OpenRead(url.LocalPath);

                // case "avares":
                //     if (!AssetLoader.Exists(url))
                //         return null;

                //     return AssetLoader.Open(url);
                case "https":
                case "http":
                    {
                        var client = new HttpClient();
                        var result = await client.GetAsync(url);
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            return result.Content.ReadAsStream();
                        }
                        return null;
                    }

                default:
                    throw new InvalidDataException($"unsupport scheme '{url.Scheme}'");
            }
        }
    }
}