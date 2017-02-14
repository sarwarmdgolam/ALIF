using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCML.Models
{
    public class RPostsViewModel
    {
        public IEnumerable<Content> RecentPosts { get; set; }
    }
}