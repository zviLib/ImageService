using ServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace ServiceWeb.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            // get service status
            ViewBag.Status = WebClient.IsServiceUp();

            List<Student> students = new List<Student>();
            StreamReader reader = null;
            // get students list
            try
            {
                string path = Server.MapPath(".");
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


            ViewBag.Sum = CountPhotos(Server.MapPath(".") + "\\Images");

            return View(students);
        }

        /// <summary>
        /// counts the number of photos in service
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private int CountPhotos(string path)
        {
            
            int num = Directory.GetFiles(path).Length;

            //scan all sub-folders
            foreach (string s in Directory.GetDirectories(path))
            {
                if (!s.Contains("thumbnail"))
                    num+= CountPhotos(s);
            }

            return num;
        }
    }
}