﻿using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Interfaces
{
    /// <summary>
    /// Load and process NYb files 
    /// </summary>
    public interface INYBLoader
    {
        /// <summary>
        /// Reads data from CSV file and maps to postalAddressDTO object
        /// </summary>
        /// <param name="line">Line read from CSV File</param>
        /// <returns>Postal Address DTO</returns>
        List<PostalAddressDTO> LoadNybDetailsFromCSV(string line);

        /// <summary>
        /// Web API call to save postalAddress to PostalAddress table
        /// </summary>
        /// <param name="lstAddress">List of mapped address dto to validate each records</param>
        /// <returns>If success returns true else returns false</returns>
        Task<bool> SaveNybDetails(List<PostalAddressDTO> lstAddress, string fileName);
    }
}
