using System.IO;
using RM.Integration.ThirdPartyAddressLocation.Loader.Utils.Interfaces;

namespace RM.Integration.ThirdPartyAddressLocation.Loader.Utils
{
    /// <summary>
    /// Move files from source folder to destibnation folder
    /// </summary>
    public class FileMover : IFileMover
    {
        /// <summary>
        /// Method to move files from source folder to destibnation folder
        /// </summary>
        /// <param name="source">Source path</param>
        /// <param name="destination">Destination path</param>
        public void MoveFile(string[] source, string[] destination)
        {
            string sourceFilePath = Path.Combine(source);
            string destinationFilePath = Path.Combine(destination);

            File.Move(sourceFilePath, destinationFilePath);
        }
    }
}
