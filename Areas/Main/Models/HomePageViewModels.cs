using AspMVC.Models;

namespace AspMVC.Areas.Main.Models
{

    public class HomePageViewModel
    {
        public IEnumerable<GameCell> gameLists { get; set; }
    }

    public class BrowseViewModel
    {
        public List<GameCell> gameCells { get; set; }

        public List<Genre> Genres { get; set; }
    }
}
