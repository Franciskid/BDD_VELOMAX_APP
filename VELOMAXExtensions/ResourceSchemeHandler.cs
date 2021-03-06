using CefSharp;
using CefSharp.Wpf;
using CefSharp;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace BDD_VELOMAX_APP.VeloMaxExtensions
{

    public class ResourceSchemeHandler : ResourceHandler
    {
        public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback)
        {
            var names = this.GetType().Assembly.GetManifestResourceNames();

            Console.WriteLine(names);

            Uri u = new Uri(request.Url);
            String file = u.Authority + u.AbsolutePath; // Note: The directory name must be all lowercase letters, otherwise the Resource will not be available

            Assembly ass = Assembly.GetExecutingAssembly();
            String resourcePath = ass.GetName().Name + "." + file.Replace("/", "."); // You can set a breakpoint to see the value here

            Task.Run(() =>
            {
                using (callback)
                {
                    if (ass.GetManifestResourceInfo(resourcePath) != null)
                    {
                        Stream stream = ass.GetManifestResourceStream(resourcePath);
                        string mimeType = "application/octet-stream";
                        switch (Path.GetExtension(file))
                        {
                            case ".html":
                                mimeType = "text/html";
                                break;
                            case ".js":
                                mimeType = "text/javascript";
                                break;
                            case ".css":
                                mimeType = "text/css";
                                break;
                            case ".png":
                                mimeType = "image/png";
                                break;
                            case ".appcache":
                                break;
                            case ".manifest":
                                mimeType = "text/cache-manifest";
                                break;
                        }

                        // Reset the stream position to 0 so the stream can be copied into the underlying unmanaged buffer
                        stream.Position = 0;
                        // Populate the response values - No longer need to implement GetResponseHeaders (unless you need to perform a redirect)
                        ResponseLength = stream.Length;
                        MimeType = mimeType;
                        StatusCode = (int)HttpStatusCode.OK;
                        Stream = stream;

                        callback.Continue();
                    }
                    else
                    {
                        callback.Cancel();
                    }
                }
            });

            return CefReturnValue.Continue;
        }
    }

    class ResourceSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new ResourceSchemeHandler();
        }

        public static string SchemeName
        {
            get
            {
                return "resource"; // Here I set SchemeName as nacollector, of course you can also change it to other
            }
        }
    }
}
