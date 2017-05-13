using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
    /// <summary>
    /// This interface contains methods for fetching OSRoadLink data
    /// </summary>
    public interface IOSRoadLinkRepository
    {
        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        string GetOSRoadLink(string toid);
    }
}
