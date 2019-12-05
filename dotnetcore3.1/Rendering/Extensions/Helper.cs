using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;

namespace Rendering.Extensions
{
    public static class Helper
    {
        public static IHtmlContent Body(Func<object, IHtmlContent> body)
        {
            return body(null);
        }
    }
}
