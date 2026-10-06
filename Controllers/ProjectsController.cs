using Microsoft.AspNetCore.Mvc;
using SusanOsuolaleCV.Models;
using System.Collections.Generic;

namespace SusanOsuolaleCV.Controllers
{
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            var projects = new List<ProjectItem>
            {
                new ProjectItem { Title = "SchoolMaster", Summary = "Multi-tenant school management backend.", Tags = new List<string>{"C#","ASP.NET Core","EF Core","PostgreSQL","Redis","Docker"}, Status = "Completed" },
                new ProjectItem { Title = "FreelanceFlow", Summary = "Invoice/payment microservice platform.", Tags = new List<string>{".NET","CQRS","RabbitMQ","PostgreSQL","Elasticsearch"}, Status = "In Progress" },
                new ProjectItem { Title = "TaskPulse", Summary = "ASP.NET Core Web API project.", Tags = new List<string>{"C#","ASP.NET Core","Redis","Docker"}, Status = "Completed" },
                new ProjectItem { Title = "AI Subscription Platform", Summary = "AI-powered subscription platform.", Tags = new List<string>{"FastAPI","OpenAI","PostgreSQL","Docker","Paystack"}, Status = "Completed" }
            };
            return View(projects);
        }
    }
}
