using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartCrawler
{
    public static class ActorSafeNameFromUri
    {
        public static string ToActorName(this Uri uri)
        {
            return Uri.EscapeDataString(uri.ToString());
        }
    }
}
