namespace ImageService.Modal
{
    public interface IImageModal
    {
        /// <summary>
        /// The Function Addes A copy of the file and thumbnail
        /// to our image system
        /// </summary>
        /// <param name="path">Path to the new file added</param>
        /// <returns>the result of the procedure</returns>
        string AddFile(string path, out bool result);

    }
}
