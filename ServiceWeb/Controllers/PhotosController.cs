using ServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace ServiceWeb.Controllers
{
    public class PhotosController : Controller
    {
        private static List<Photo> photos;

        // GET: Photos
        public ActionResult Index()
        {
            photos = new List<Photo>();

            string path = Server.MapPath(".") + "\\Images";

            ScanFolder(path);

            return View(photos);
        }
        

        private void ScanFolder(string path)
        {
            //scan all files
            foreach (string s in Directory.GetFiles(path))
            {
                photos.Add(ParsePath(s));
            }

            //scan all sub-folders
            foreach (string s in Directory.GetDirectories(path))
            {
                if (!s.Contains("thumbnail"))
                    ScanFolder(s);
            }
        }

        private Photo ParsePath(string path)
        {
            string[] split = path.Split('\\');
            int length = split.Length;
            // if time is unknown
            if (split[length-2].Equals("UnknownTime"))
            {
                return new Photo
                {
                    PicPath = "Images\\UnknownTime\\" + Path.GetFileName(path),
                    ThumbPath = "Images\\thumbnail\\UnknownTime\\" + Path.GetFileName(path),
                    Label = Path.GetFileNameWithoutExtension(path),
                    TakenMonth = "Unknown Time",
                    TakenYear = ""
                };
            }

            //if time is known
                return new Photo
                {
                    Label = Path.GetFileNameWithoutExtension(path),
                    TakenMonth = split[length - 2],
                    TakenYear = split[length-3],
                    PicPath = "Images\\"+ split[length - 3] + "\\" + split[length - 2] + "\\" + Path.GetFileName(path),
                    ThumbPath = "Images\\thumbnail\\" + split[length - 3] + "\\" + split[length - 2] + "\\" + Path.GetFileName(path)
                };
        }
    }
}