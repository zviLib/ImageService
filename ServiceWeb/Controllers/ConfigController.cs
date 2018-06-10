using Newtonsoft.Json.Linq;
using ServiceWeb.Models;
using SharedInfo.Commands;
using SharedInfo.Messages;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ServiceWeb.Controllers
{
    public class ConfigController : Controller
    {
        public static string OutputDir { private set; get; }
        public static string SourceName { private set; get; }
        public static string LogName { private set; get; }
        public static string ThumbSize { private set; get; }
        public static List<string> handlers;
        
        public ActionResult Index()
        {
            if (handlers == null)
            {
                Dictionary<int, string> configs = WebClient.getAppConfig();

                handlers = new List<string>();

                foreach (KeyValuePair<int, string> val in configs)
                {
                    switch (val.Key)
                    {
                        case (int)AppConfigValuesEnum.OutputDirectory:
                            OutputDir = val.Value;
                            break;
                        case (int)AppConfigValuesEnum.SourceName:
                            SourceName = val.Value;
                            break;
                        case (int)AppConfigValuesEnum.LogName:
                            LogName = val.Value;
                            break;
                        case (int)AppConfigValuesEnum.ThumbnailSize:
                            ThumbSize = val.Value;
                            break;
                        default:
                            handlers.Add(val.Value);
                            break;
                    }
                }

            }

            ViewBag.list = handlers;
            ViewBag.Output = OutputDir;
            ViewBag.Source = SourceName;
            ViewBag.Log = LogName;
            ViewBag.Thumb = ThumbSize;


            return View();
        }

        public ActionResult DeleteHandler()
        {
            return View();
        }

        [HttpPost]
        public JObject RemoveHandler(string path)
        {
            WebClient.SendCommand(new CommandRecievedEventArgs
            {
                Type = CommandEnum.CloseCommand,
                Args = new string[] { path }
            });

            DirectoryCloseEventArgs args = WebClient.ReadCloseCommand();

            if (handlers != null)
                handlers.Remove(args.Path);

            return null;
           // return RedirectToAction("Config");
        }
    }
}