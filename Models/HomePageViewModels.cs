using AspMVC.Models.EF;
using X.PagedList;

namespace AspMVC.Models
{

    public class HomePageViewModel
    {
        public List<GameCell> featured { get; set; }
        public List<GameCell> lastest { get; set; }
        public List<GameCell> recentPopuplar { get; set; }
        public List<GameCell> recentHighrated { get; set; }

    }

    public class BrowseViewModel
    {
        public IPagedList<GameCell> GameCells { get; set; }

        public List<Genre> Genres { get; set; }

        public List<Platform> Platforms { get; set; }
    }
}
