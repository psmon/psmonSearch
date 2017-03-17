using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psmonSearch.viewModels
{
    public class SearchReq
    {
        public string Keyword  { get;set; }
        public string Method { get; set; }

    }

    public class CrawlWebReq
    {
        public string Url { get; set; }
    }
}
