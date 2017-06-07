namespace RM.Integration.ThirdPartyAddressLocation.Loader.Utils.Interfaces
{
    /// <summary>
    ///  Interface move files from source folder to destibnation folder
    /// </summary>
    public interface IFileMover
    {
        /// <summary>
        /// Method to move files from source folder to destibnation folder
        /// </summary>
        /// <param name="source">Source path</param>
        /// <param name="destination">Destination path</param>
        void MoveFile(string[] source, string[] destination);
    }
}
