using System;
using System.Collections.Generic;
using System.Text;

namespace Rendering
{
    public interface IHostingOptions
    {
        string ClientBasePath { get; set; }
        string ServerBasePath { get; set; }
    }
}
