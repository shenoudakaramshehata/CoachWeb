using System;
using System.Collections.Generic;

#nullable disable

namespace Coach.Models
{
    public partial class PageContent
    {
        public int PageContentId { get; set; }
        public string PageTitleAr { get; set; }
        public string ContentAr { get; set; }
        public string PageTitleEn { get; set; }
        public string ContentEn { get; set; }
    }
}
