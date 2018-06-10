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
        private static Regex r;                   // Used for DateTime function

        public ImageModal(string outPutFolder, int thumbnailSize)
        {
            m_OutputFolder = outPutFolder;
            m_thumbnailSize = thumbnailSize;
            r = new Regex(":");
        }

        public string AddFile(string path, out bool result)
        {
            // get picture name
            string name = path.Substring(path.LastIndexOf("\\"));

            //path builders
            StringBuilder picPath = new StringBuilder(m_OutputFolder);
            StringBuilder thumbPath = new StringBuilder(m_OutputFolder + "\\thumbnail");

            Image image = null, thumb = null;
            try
            {
                //create output folders if not exists
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
                ///if date is available
                if (dateTime.Year != 1 || dateTime.Month != 1)
                {
                    ///create year path
                    picPath.Append("\\" + dateTime.Year);
                    thumbPath.Append("\\" + dateTime.Year);
                    if (!System.IO.Directory.Exists(picPath.ToString()))
                        System.IO.Directory.CreateDirectory(picPath.ToString());
                    if (!System.IO.Directory.Exists(thumbPath.ToString()))
                        System.IO.Directory.CreateDirectory(thumbPath.ToString());
                    ///create month path
                    picPath.Append("\\" + dateTime.Month);
                    thumbPath.Append("\\" + dateTime.Month);
                    if (!System.IO.Directory.Exists(picPath.ToString()))
                        System.IO.Directory.CreateDirectory(picPath.ToString());
                    if (!System.IO.Directory.Exists(thumbPath.ToString()))
                        System.IO.Directory.CreateDirectory(thumbPath.ToString());
                }
                else // if there's no taken time for the photo
                {
                    picPath.Append("\\UnknownTime");
                    thumbPath.Append("\\UnknownTime");
                    if (!System.IO.Directory.Exists(picPath.ToString()))
                        System.IO.Directory.CreateDirectory(picPath.ToString());
                    if (!System.IO.Directory.Exists(thumbPath.ToString()))
                        System.IO.Directory.CreateDirectory(thumbPath.ToString());
                }

                //open image
                image = Image.FromFile(path);

                //copy image to path
                image.Save(picPath.ToString() + name);

                //create thumbnail
                thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                thumb.Save(thumbPath.ToString() + name);
                thumb.Dispose();
                image.Dispose();

                result = true;
                return String.Format("{2} copied successfully to:  {0}\nThumbnail copied successfully to: {1}", picPath.ToString(), thumbPath.ToString(),name.Substring(1));
            }
            catch (Exception e)
            {
                result = false;
                //release resources
                if (thumb != null)
                    thumb.Dispose();
                if (image != null)
                    image.Dispose();
                return "Image copying failed: " + e.Message;
            }
        }




        /// <summary>
        /// retrieves the datetime when the image was taken
        /// </summary>
        /// <param name="path">path to the image</param>
        /// <returns>the datetime when the image was taken, 1\1\1 if information unavailable</returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            Image myImage = Image.FromStream(fs, false, false);

            DateTime dateTime;

            try
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                dateTime = DateTime.Parse(dateTaken);
            }
            catch (ArgumentException)
            {
                dateTime = new DateTime(1, 1, 1);

            }
            finally
            {
                //release resources
                myImage.Dispose();
                fs.Close();
            }

            return dateTime;

        }

        #endregion

    }
}
