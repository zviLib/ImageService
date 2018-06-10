using ServiceWeb.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace ServiceWeb.Controllers
{
    public class PhotosController : Controller
    {
        private static List<Photo> photos;
        private static int ID = 1;

        // GET: Photos
        public ActionResult Index()
        {
            photos = new List<Photo>();

            string path = Server.MapPath(".") + "\\Images";

            ID = 1;

            ScanFolder(path);

            return View(photos);
        }

        public ActionResult Delete(int id)
        {
            if (photos == null)
                return RedirectToAction("Index");

            bool found = false;

            foreach (Photo p in photos)
            {
                if (p.ID == id)
                {
                    found = true;
                    ViewBag.ThumbPath = "..\\..\\" + p.ThumbPath;
                    ViewBag.Label = p.Label;
                    ViewBag.TakenMonth = p.TakenMonth;
                    ViewBag.TakenYear = p.TakenYear;
                    ViewBag.ID = p.ID;
                    break;
                }
            }

            if (!found)
                return RedirectToAction("Index");

            return View();
        }

        public ActionResult DeletePhoto(int id)
        {
            if (photos == null)
                return RedirectToAction("Index");

            foreach (Photo p in photos)
            {
                string serverpath = Server.MapPath("..\\..");

                if (p.ID == id)
                {
                    System.IO.File.Delete(serverpath + "\\" + p.PicPath);
                    System.IO.File.Delete(serverpath + "\\" + p.ThumbPath);
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult View(int id)
        {
            if (photos == null)
                return RedirectToAction("Index");

            bool found = false;

            foreach (Photo p in photos)
            {
                if (p.ID == id)
                {
                    found = true;
                    ViewBag.PicPath = "..\\..\\" + p.PicPath;
                    ViewBag.Label = p.Label;
                    ViewBag.TakenMonth = p.TakenMonth;
                    ViewBag.TakenYear = p.TakenYear;
                    ViewBag.ID = p.ID;
                    break;
                }
            }

            if (!found)
                return RedirectToAction("Index");

            return View();
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
            if (split[length - 2].Equals("UnknownTime"))
            {
                return new Photo
                {
                    ID = PhotosController.ID++,
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
                ID = PhotosController.ID++,
                PicPath = "Images\\" + split[length - 3] + "\\" + split[length - 2] + "\\" + Path.GetFileName(path),
                ThumbPath = "Images\\thumbnail\\" + split[length - 3] + "\\" + split[length - 2] + "\\" + Path.GetFileName(path),
                Label = Path.GetFileNameWithoutExtension(path),
                TakenMonth = split[length - 2],
                TakenYear = split[length - 3]
            };
        }
    }
}