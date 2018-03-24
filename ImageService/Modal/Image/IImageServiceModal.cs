using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public interface IImageServiceModal
    {
        /// <summary>
        /// The Function Addes A file to the system
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <returns>Indication if the Addition Was Successful</returns>
        string AddFile(string path, out bool result);

        /// <summary>
        /// The Function Moves A file already exists in the system
        /// </summary>
        /// <param name="src">The Path of the Image from the file</param>
        /// <param name="dst">The Path to the new location from the file</param>
        /// <returns>Indication if the Addition Was Successful</returns>
        string MoveFile(string src, string dst, out bool result);

        /// <summary>
        /// The Function Creates a new Directory in the system
        /// </summary>
        /// <param name="path">Where to add the folder</param>
        /// <param name="name">The name of the folder</param>
        /// <returns>Indication if the Addition Was Successful</returns>
        string CreateFolder(string path, string name, out bool result);
    }
}
