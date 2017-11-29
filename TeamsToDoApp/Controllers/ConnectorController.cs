using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamsToDoApp.DataModel;
using TeamsToDoApp.Repository;

namespace TeamsToDoApp.Controllers
{
    public class ConnectorController : Controller
    {
        // GET: Setup page
        public ViewResult Setup()
        {
            return View();
        }

        // GET: Register Callback
        public ActionResult Register()
        {
            var error = Request["error"];
            var state = Request["state"];
            if (!String.IsNullOrEmpty(error))
            {
                return RedirectToAction("Error");
            }
            else
            {
                var group = Request["group_name"];
                var webhook = Request["webhook_url"];

                Subscription sub = new Subscription();
                sub.GroupName = group;
                sub.WebHookUri = webhook;

                // Save the subscription so that it can be used to push data to the registered channels.
                SubscriptionRepository.Subscriptions.Add(sub);

                return View();
            }
        }

        // Error page
        public ActionResult Error()
        {
            return View();
        }
    }
}