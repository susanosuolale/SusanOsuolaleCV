using System.Collections.Generic;

namespace SusanOsuolaleCV.Models
{
    public class WorkItem
    {
        public string Title { get; set; } = "";
        public string Company { get; set; } = "";
        public string Period { get; set; } = "";
        public List<string> Responsibilities { get; set; } = new();
    }
}
