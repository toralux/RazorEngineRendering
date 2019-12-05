using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Rendering.Models;

namespace Rendering
{
    public class RenderingApi : IRenderingApi
    {
        private readonly IHostingOptions _hostingOptions;
        private readonly ViewRender _viewRender;

        public RenderingApi(IHostingOptions hostingOptions, ViewRender viewRender)
        {
            _hostingOptions = hostingOptions;
            _viewRender = viewRender;
        }

        public async Task<string> RenderContent<T>(T content, string name = null) where T : ContentBase
        {
            var renderData = new RenderBase<T>(_hostingOptions, content);

            var html = await _viewRender.Render<RenderBase<T>, T>(name ?? typeof(T).Name, renderData);

            return html;
        }
    }
}
