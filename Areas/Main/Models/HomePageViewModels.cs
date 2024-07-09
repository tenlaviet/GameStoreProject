using AspMVC.Models;
using AspMVC.Models.EF;

namespace AspMVC.Areas.Main.Models
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
        public List<GameCell> GameCells { get; set; }

        public List<Genre> Genres { get; set; }

        public List<Platform> Platforms { get; set; }
    }
}
