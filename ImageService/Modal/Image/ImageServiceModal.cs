﻿using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        public string AddFile(string path, out bool result)
        {
            throw new NotImplementedException();
        }

        public string CreateFolder(string path, string name, out bool result)
        {
            throw new NotImplementedException();
        }

        public string MoveFile(string src, string dst, out bool result)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
