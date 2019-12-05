using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Rendering.Models;

namespace Rendering
{
    public interface IRenderingApi
    {
        Task<string> RenderContent<T>(T content, string name = null) where T : ContentBase;
    }
}
