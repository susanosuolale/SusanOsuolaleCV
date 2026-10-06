using Microsoft.AspNetCore.Mvc;
using SusanOsuolaleCV.Models;
using System.Collections.Generic;

namespace SusanOsuolaleCV.Controllers
{
    public class ExperienceController : Controller
    {
        public IActionResult Index()
        {
            var list = new List<WorkItem>
            {
                new WorkItem { Title = "Backend Software Developer", Company = "Self-Directed / Personal Projects", Period = "Feb 2026 - Present", Responsibilities = new System.Collections.Generic.List<string> {
                    "C# and ASP.NET Core for building REST APIs",
                    "SQL Server, PostgreSQL, EF Core",
                    "Redis, Docker, Clean Architecture, Microservices",
                    "Background processing and messaging"
                } },
                new WorkItem { Title = "Data Analyst / Data Scientist", Company = "Various", Period = "2023 - 2024", Responsibilities = new System.Collections.Generic.List<string> {
                    "Python, Pandas, NumPy, Scikit-learn",
                    "Exploratory data analysis and visualization",
                    "Statistical analysis and machine learning"
                } },
                new WorkItem { Title = "Lead Generator", Company = "Freelance", Period = "2024 - 2025", Responsibilities = new System.Collections.Generic.List<string> {
                    "Researched and identified potential leads",
                    "Maintained prospect lists for outreach"
                } },
                new WorkItem { Title = "Building Engineer", Company = "MEE Consults", Period = "2023", Responsibilities = new System.Collections.Generic.List<string> {
                    "Electrical distribution and lighting systems",
                    "Emergency power and low-voltage systems",
                    "AutoCAD electrical layouts and coordination"
                } }
            };

            return View(list);
        }
    }
}
