namespace AspMVC.Areas.Main.Models
{
    public class GameCell
    {
        public string name { get; set; }
        public string shortDescription { get; set; }
    }
    public class HomePageViewModel
    {
        public IEnumerable<GameCell> gameLists { get; set; }
    }
}
