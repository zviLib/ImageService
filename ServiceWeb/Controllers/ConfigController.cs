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
        public static List<Handler> handlers;
        private static int ID = 1;

        public ActionResult Index()
        {
            if (handlers == null)
            {
                Dictionary<int, string> configs = WebClient.getAppConfig();

                handlers = new List<Handler>();

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
                            handlers.Add(new Handler
                            {
                                ID = ConfigController.ID++,
                                Path = val.Value
                            });
                            break;
                    }
                }

            }
            
            ViewBag.Output = OutputDir;
            ViewBag.Source = SourceName;
            ViewBag.Log = LogName;
            ViewBag.Thumb = ThumbSize;


            return View(handlers);
        }

        public ActionResult Delete(int id)
        {
            if (handlers == null)
                return RedirectToAction("Index");

            foreach (Handler h in handlers)
            {
                if (h.ID == id)
                {
                    ViewBag.path = h.Path;
                    ViewBag.id = h.ID;
                    return View();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveHandler(int id)
        {
            if (handlers == null)
                return RedirectToAction("Index");

            Handler res = null;
            foreach (Handler h in handlers)
            {
                if (h.ID == id)
                {
                    res = h;
                    break;
                }
            }

            if (res == null)
                return RedirectToAction("Index");

            Writer.WriteLine("Sending command:" + res.Path);
            WebClient.SendCommand(new CommandRecievedEventArgs
            {
                Type = CommandEnum.CloseCommand,
                Args = new string[] { res.Path }
            });

            Writer.WriteLine("Sent command:" + res.Path);
            DirectoryCloseEventArgs args = WebClient.ReadCloseCommand();

            res = null;
            foreach (Handler h in handlers)
            {
                if (h.Path.Equals(args.Path))
                {
                    res = h;
                    break;
                }

            }
            if (res != null)
                handlers.Remove(res);


            return RedirectToAction("Index");
        }
    }
}