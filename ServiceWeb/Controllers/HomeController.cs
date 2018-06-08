using Newtonsoft.Json.Linq;
using ServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using SharedInfo.Commands;
using SharedInfo.Messages;

namespace ServiceWeb.Controllers
{
    public class HomeController : Controller
    {

        public static List<string> handlers;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImageWeb()
        {
            // get service status
            ViewBag.Status = WebClient.IsServiceUp();

            List<Student> students = new List<Student>();
            StreamReader reader = null;
            // get students list
            try
            {
                string path = Server.MapPath("..");
                reader = new StreamReader(path + "/App_Data/details.txt");
                string line;
                string[] details;
                while ((line = reader.ReadLine()) != null)
                {
                    details = line.Split(' ');
                    students.Add(new Student
                    {
                        Firstname = details[0],
                        Lastname = details[1],
                        ID = int.Parse(details[2])
                    });

                }

                reader.Close();
            }
            catch (Exception e)
            {
                students.Add(new Student
                {
                    Firstname = e.Message,
                    ID = -1
                });

                if (reader != null)
                    reader.Close();
            }


            ViewBag.Sum = 15;

            return View(students);
        }

        public ActionResult Config()
        {
            if (handlers == null)
            {
                Dictionary<int, string> configs = WebClient.getAppConfig();

                handlers = new List<string>();

                foreach (KeyValuePair<int, string> val in configs)
                    if (val.Key > 3)
                        handlers.Add(val.Value);

                WebClient.DirectoryClosed += DirectoryClosed;
            }

            ViewBag.list = handlers;


            return View();
        }

        public ActionResult DeleteHandler()
        {
            return View();
        }

        public ActionResult RemoveHandler(string path)
        {
            WebClient.SendCommand(new CommandRecievedEventArgs
            {
                Type = CommandEnum.CloseCommand,
                Args = new string[] { path }
            });

            return RedirectToAction("Config");
        }

        public void DirectoryClosed(object sender, DirectoryCloseEventArgs args)
        {
            if (handlers != null)
                handlers.Remove(args.Path);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}