using System;
using System.Text;

namespace Rendering.Models
{

    public class ConfirmEmail : ContentBase
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Code { get; set; }
    }
}
