using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using TeamsSampleTaskApp.DataModel;
using TeamsToDoApp.DataModel;
using TeamsToDoApp.Repository;

namespace TeamsToDoApp.Controllers
{
    /// <summary>
    /// Represents the controller which handles tasks.
    /// </summary>
    public class TaskController : Controller
    {

        [Route("task/index")]
        [HttpGet]
        public ActionResult Index()
        {
            return View(TaskRepository.Tasks);
        }

        [Route("task/create")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Route("task/create")]
        [HttpPost]
        public async Task<ActionResult> Create(TaskItem item)
        {
            item.Guid = Guid.NewGuid().ToString();
            TaskRepository.Tasks.Add(item);

            // Loop through subscriptions and notify each channel that task is created.
            foreach (var sub in SubscriptionRepository.Subscriptions)
            {
                await TeamsSampleTaskApp.Utils.Utils.CallWebhook(sub.WebHookUri, item);
            }

            return RedirectToAction("Detail", new { id = item.Guid });
        }

        [Route("task/detail/{id}")]
        [HttpGet]
        public ActionResult Detail(string id)
        {
            return View(TaskRepository.Tasks.FirstOrDefault(i => i.Guid == id));
        }

        [Route("task/update")]
        [HttpPost]
        public HttpResponseMessage Update([System.Web.Http.FromBody]Request request, string id)
        {
            var task = TaskRepository.Tasks.First(t => t.Guid == id);
            task.Title = request.Title;

            HttpResponseMessage response = new HttpResponseMessage();
            response.Headers.Add("CARD-ACTION-STATUS", "true");
            return response;
        }
    }
}