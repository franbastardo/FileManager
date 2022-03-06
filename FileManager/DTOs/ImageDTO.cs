using System.Collections.Generic;

namespace FileManager.DTOs
{
    public class ImageDTO
    {
        public List<Image> Results;
    }
    public class Image
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Links Links { get; set; }
    }

    public class Links
    {
        public string Download { get; set; }
    }
}
