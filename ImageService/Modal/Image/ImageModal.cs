using System;
using System.IO;
using System.Text;

namespace ImageService.Modal
{
    public class ImageModal : IImageModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        
        public string AddFile(string path, out bool result)
        {
            //check if output folder exists
            if (!System.IO.Directory.Exists(m_OutputFolder))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                } catch(Exception e)
                {
                    result = false;
                    return null;
                }
            }

                try { 
            System.IO.File.Copy(path, m_OutputFolder, true);
            } catch (Exception e)
            {
                result= false;
                return null;
            }

            result = true;
            return m_OutputFolder;
        }

        //retrieves the datetime WITHOUT loading the whole image
        private static DateTime GetDateTakenFromImage(string path)
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
