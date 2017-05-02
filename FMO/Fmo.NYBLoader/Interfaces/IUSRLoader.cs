namespace Fmo.NYBLoader.Interfaces
{
    /// <summary>
    /// Load third party file, process and add to MSMQ
    /// </summary>
    public interface IUSRLoader
    {

        /// <summary>
        /// Load the XML data from file to Message Queue.
        /// </summary>
        /// <param name="strPath"></param>
        void LoadUSRDetailsFromXML(string strPath);
    }
}
