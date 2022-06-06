using System.Collections.Generic;
using ProjectLibrary.Database;

namespace Template_WatchShop.Models
{
    public class DetailWatch
    {
        public Watch watch { get; set; }
        public string MenuAlias { get; set; }
        public string TypeWatch { get; set; }
        public List<ShowObject> watches { get; set; }
        public List<WatchGallery> watchGalleries { get; set; }
    }
}