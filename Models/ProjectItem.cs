using System.Collections.Generic;

namespace SusanOsuolaleCV.Models
{
    public class ProjectItem
    {
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
        public List<string> Tags { get; set; } = new();
        public string Status { get; set; } = "";
    }
}
