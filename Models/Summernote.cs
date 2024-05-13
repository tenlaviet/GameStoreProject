namespace AspMVC.Models
{
    public class Summernote
    {
        public Summernote(string iDEditor, bool loadLibrary,int height, int width)
        {
            IDEditor = iDEditor;
            LoadLibrary = loadLibrary;
        }

        public string IDEditor { get; set; }

        public bool LoadLibrary { get; set; }

        public int height { get; set; } = 120;
        public int width { get; set; } = 550;

        public string toolbar { get; set; } = @"
            [
                ['style', ['style']],
                ['font', ['bold', 'underline', 'clear']],
                ['color', ['color']],
                ['insert', ['link', 'picture', 'video']],
                ['view', ['codeview']]
            ]       
        ";
    }
}
