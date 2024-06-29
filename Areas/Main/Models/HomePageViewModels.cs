using AspMVC.Models;
using AspMVC.Models.EF;

namespace AspMVC.Areas.Main.Models
{

    public class HomePageViewModel
    {
        public IEnumerable<GameCell> gameLists { get; set; }
    }

    public class BrowseViewModel
    {
        public List<GameCell> GameCells { get; set; }

        public List<Genre> Genres { get; set; }

        public List<Platform> Platforms { get; set; }
    }
}
