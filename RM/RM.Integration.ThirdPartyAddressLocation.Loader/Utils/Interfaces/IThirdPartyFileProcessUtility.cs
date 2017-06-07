namespace RM.Integration.ThirdPartyAddressLocation.Loader.Utils.Interfaces
{
    /// <summary>
    /// Load third party file, process and add to MSMQ
    /// </summary>
    public interface IThirdPartyFileProcessUtility
    {

        /// <summary>
        /// Load the XML data from file to Message Queue.
        /// </summary>
        /// <param name="strPath"></param>
        void LoadUSRDetailsFromXML(string strPath);
    }
}
