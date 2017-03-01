using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Owin;

namespace psmonSearch
{
    class WebStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
