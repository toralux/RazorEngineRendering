
namespace Rendering.Models
{
    public class RenderBase<T> : IHostingOptions where T : ContentBase
    {
        public string ClientBasePath { get; set; }
        public string ServerBasePath { get; set; }

        public T Content { get; set; }

        public RenderBase(IHostingOptions hostingOptions, T content)
        {
            ClientBasePath = hostingOptions.ClientBasePath;
            ServerBasePath = hostingOptions.ServerBasePath;
            Content = content;
        }
    }
}
