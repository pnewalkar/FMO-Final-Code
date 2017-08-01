using System.Collections.Generic;
using RM.Operational.RouteLog.WebAPI.DTO;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    /// <summary>
    /// Helper class used in conjunction with RouteLogSummaryFactory to hold groups of sequenced points
    /// </summary>
    internal class RouteLogSummaryGroup
    {
        /// <summary>
        /// Group type enumeration
        /// </summary>
        public enum GroupType
        {
            ConsecutiveAscending,
            ConsecutiveDescending,
            EvensAscending,
            EvensDescending,
            OddsAscending,
            OddsDescending,
            Unknown
        }



    

        /// <summary>
        /// Constructs route log summary group with an initial address
        /// </summary>
        /// <param name="address">The address</param>
        public RouteLogSummaryGroup(RouteLogSequencedPointsDTO address)
        {
            // Default the group type
            this.CurrentGroupType = GroupType.Unknown;

            // Add the address to the group
            this.AddressList.Add(address);

            // Update the last address and first and last building numbers
            this.LastAddress = address;
            this.FirstBuildingNumber = address.BuildingNumber;
            this.LastBuildingNumber = null;

            // Initialize the delivery point count
            this.DeliveryPointCount = 1;

            // Initialize the multiple occupancy count
            this.MultipleOccupancy = address.MultipleOccupancy;
            this.StreetName = address.StreetName;
            this.SubBuildingName = address.SubBuildingName;
            this.BuildingName = address.BuildingName;
        }



        /// <summary>
        /// Gets or sets the current group type
        /// </summary>
        public GroupType CurrentGroupType { get; set; }



        /// <summary>
        /// Gets or sets the last address in the group
        /// </summary>
        public RouteLogSequencedPointsDTO LastAddress { get; set; }



        /// <summary>
        /// Gets the description for the group
        /// </summary>
        public string Description
        {
            get
            {
                string building = string.Empty;

                // If the increment mode is unknown or either the first or last building numbers are not set
                if (this.CurrentGroupType == GroupType.Unknown || !this.FirstBuildingNumber.HasValue || !this.LastBuildingNumber.HasValue)
                {
                    if (this.AddressList.Count == 1 && this.LastAddress != null)
                    {
                        building = GetBuildingDescription(this.LastAddress.SubBuildingName, this.LastAddress.BuildingName, this.LastAddress.BuildingNumber.ToString(), this.LastAddress.StreetName);
                    }
                    else
                    {
                        // The group is in an unexpected state
                        building = "#ERROR# Unexpected state";
                    }
                }
                else
                {
                    building = $"{this.FirstBuildingNumber.Value} to {this.LastBuildingNumber.Value} ";
                }

                return building + $"{GetGroupTypeDescription(this.CurrentGroupType)}";
            }
        }



        /// <summary>
        /// Gets the group type description for a specified group type
        /// </summary>
        /// <param name="groupType"></param>
        /// <returns>The group type description</returns>
        private string GetGroupTypeDescription(GroupType groupType)
        {
            // Get the description for the group type
            string description = string.Empty;
            switch (groupType)
            {
                case GroupType.EvensAscending:
                    description = "Evens";
                    break;

                case GroupType.EvensDescending:
                    description = "Evens";
                    break;

                case GroupType.OddsAscending:
                    description = "Odds";
                    break;

                case GroupType.OddsDescending:
                    description = "Odds";
                    break;

                case GroupType.ConsecutiveAscending:
                    description = "Cons";
                    break;

                case GroupType.ConsecutiveDescending:
                    description = "Cons";
                    break;

                case GroupType.Unknown:
                    description = string.Empty;
                    break;
            }

            // Return the description
            return description;
        }



        /// <summary>
        /// Gets the building description from the sub name, name, number and thoroughfare
        /// </summary>
        /// <param name="subName">The building sub name</param>
        /// <param name="name">The building name</param>
        /// <param name="number">The building number</param>
        /// <param name="thoroughfare">The thoroughfare on which the building is located</param>
        /// <returns>The building description</returns>
        public static string GetBuildingDescription(string subName, string name, string number, string thoroughfare)
        {
            // Remove any leading or trailing whitespace from the arguments
            subName = (subName + string.Empty).Trim();
            name = (name + string.Empty).Trim();
            number = (number + string.Empty).Trim();
            thoroughfare = (thoroughfare + string.Empty).Trim();

            // Construct the description from the sub name, name, number and thoroughfare
            string description = string.Empty;
            const string delimiter = ", ";
            if (subName.Length > 0)
            {
                description = description + subName + delimiter;
            }

            if (name.Length > 0)
            {
                description = description + name + delimiter;
            }

            if (number.Length > 0)
            {
                description = description + number + delimiter;
            }

            // Remove any surplus trailing delimiters
            while (description.EndsWith(", "))
            {
                description = description.Substring(0, description.Length - delimiter.Length);
            }

            // Return the description
            return description;
        }



        /// <summary>
        /// The list of addresses in the group
        /// </summary>
        private List<RouteLogSequencedPointsDTO> addressList = new List<RouteLogSequencedPointsDTO>();



        /// <summary>
        /// Gets the address list
        /// </summary>
        public List<RouteLogSequencedPointsDTO> AddressList
        {
            get { return addressList; }
        }



        /// <summary>
        /// Gets or sets the delivery point count
        /// </summary>
        public int DeliveryPointCount { get; set; }



        /// <summary>
        /// Gets or sets the multiple occupancy count
        /// </summary>
        public int? MultipleOccupancy { get; set; }



        /// <summary>
        /// Gets or sets the first building number
        /// </summary>
        public short? FirstBuildingNumber { get; set; }



        /// <summary>
        /// Gets or sets the last building number
        /// </summary>
        public short? LastBuildingNumber { get; set; }



        /// <summary>
        /// Gets or sets the street name
        /// </summary>
        public string StreetName { get; set; }



        /// <summary>
        /// Gets or sets the sub-building name
        /// </summary>
        public string SubBuildingName { get; set; }



        /// <summary>
        /// Gets or sets the building name
        /// </summary>
        public string BuildingName { get; set; }
    }
}