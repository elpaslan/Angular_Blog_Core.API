using System;
using System.Collections.Generic;

#nullable disable

namespace AngularBlog.Core.Api.Models
{
    public partial class BlogCategory
    {
        public BlogCategory()
        {
            Articles = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
