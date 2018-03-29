using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageService.Modal
{
    public class ImageModal : IImageModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        public ImageModal(string outPutFolder, int thumbnailSize)
        {
            m_OutputFolder = outPutFolder;
            m_thumbnailSize = thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            StringBuilder picPath = new StringBuilder(m_OutputFolder);
            StringBuilder thumbPath = new StringBuilder(m_OutputFolder + "/thumbnail");

            try
            {
                //create output folders if not exist exists
                if (!System.IO.Directory.Exists(picPath.ToString()))
                {
                    DirectoryInfo di = System.IO.Directory.CreateDirectory(picPath.ToString());
                    //set folder as hidden
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }
                if (!System.IO.Directory.Exists(thumbPath.ToString()))
                    System.IO.Directory.CreateDirectory(thumbPath.ToString());

                //get date of when picture was taken
                DateTime dateTime = GetDateTakenFromImage(path);

                //create year and month path
                ///create year path
                picPath.Append("/" + dateTime.Year);
                thumbPath.Append("/" + dateTime.Year);
                if (!System.IO.Directory.Exists(picPath.ToString()))
                    System.IO.Directory.CreateDirectory(picPath.ToString());
                if (!System.IO.Directory.Exists(thumbPath.ToString()))
                    System.IO.Directory.CreateDirectory(thumbPath.ToString());
                ///create month path
                picPath.Append("/" + dateTime.Month);
                thumbPath.Append("/" + dateTime.Month);
                if (!System.IO.Directory.Exists(picPath.ToString()))
                    System.IO.Directory.CreateDirectory(picPath.ToString());
                if (!System.IO.Directory.Exists(thumbPath.ToString()))
                    System.IO.Directory.CreateDirectory(thumbPath.ToString());

                //copy image to path
                System.IO.File.Copy(path, picPath.ToString(), true);
                
                //create thumbnail
                Image image = Image.FromFile(path);
                Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                thumb.Save(thumbPath.ToString());

                result = true;
                return "Image copied successfully to: "+ picPath.ToString();
            }
            catch (Exception e)
            {
                result = false;
                return "Image copying failed: " + path;
            }
        }


        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        //retrieves the datetime WITHOUT loading the whole image
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        #endregion

    }
}
