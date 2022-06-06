using System.Collections.Generic;
using ProjectLibrary.Database;

namespace Template_WatchShop.Models
{
    public class DetailArticle
    {
        public Article Article { get; set; }
        public List<Article> Articles { get; set; }
        public string AliasMenu { get; set; }
    }
}