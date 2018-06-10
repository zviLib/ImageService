using System.Collections.Generic;
using System.Web.Mvc;
using ServiceWeb.Models;

namespace ServiceWeb.Controllers
{
    public class LogsController : Controller
    {
        private static List<Log> logs;
        private static List<Log> filterLogs;

        // GET: Logs
        public ActionResult Index()
        {
            logs = WebClient.GetLogs();
            filterLogs = logs;

            return View(logs);
        }

        public ActionResult Filtered()
        {
            if (filterLogs == null)
                filterLogs = new List<Log>();

            return View(filterLogs);
        }

        [HttpPost]
        public void Filter(string input)
        {
            if (input==null||input.Equals(""))
            {
                filterLogs = logs;
                return;
            }
                


            filterLogs = new List<Log>();

            foreach (Log log in logs)
            {
                if (log.Type.ToString().Equals(input))
                    filterLogs.Add(log);
            }

            return;

        }
    }
}