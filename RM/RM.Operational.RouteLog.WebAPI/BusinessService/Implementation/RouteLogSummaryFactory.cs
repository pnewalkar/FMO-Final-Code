using RM.Operational.RouteLog.WebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RM.Operational.RouteLog.WebAPI.BusinessService.RouteLogSummaryGroup;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    /// <summary>
    /// Helper class that groups sequenced delivery points according to the characteristics and sequencing of
    ///   the building number and name
    /// Used in conjunction with the RouteLogSummaryGroup class
    /// </summary>
    public static class RouteLogSummaryFactory
    {
        /// <summary>
        /// Attempts to add the specified address to the group
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="group">The group</param>
        /// <returns>True if the address could be added to the group, otherwise false</returns>
        private static bool Add(RouteLogSequencedPointsDTO address, RouteLogSummaryGroup group)
        {
            // Determine whether the address should be part of the group
            //
            // Assume that the address is not part of the group
            bool canAdd = false;

            // If the group type is unknown
            if (group.CurrentGroupType == GroupType.Unknown)
            {
                // Identify the group type from the address
                group.CurrentGroupType = IdentifyGroupType(address, group);

                // The address can be added if the group type is known
                canAdd = group.CurrentGroupType != GroupType.Unknown;
            }
            else
            {
                // The address can be added if it continues the sequence predicted by the group type
                canAdd = ContinuesGroup(address, group);
            }

            // If the address can be added
            if (canAdd)
            {
                // Add the address to the group
                group.AddressList.Add(address);

                // Update the last address and last building number
                group.LastAddress = address;
                group.LastBuildingNumber = address.BuildingNumber;

                // Update the delivery point count
                group.DeliveryPointCount++;

                // Update the multiple occupancy count if the address has multiple occupancy
                group.MultipleOccupancy = group.MultipleOccupancy + address.MultipleOccupancy;
            }

            // Return whether the address can be added to the group
            return canAdd;
        }



        /// <summary>
        /// This method is used to get the sequenced points summary
        /// </summary>
        /// <param name="addressList">addressList as List of route sequenced Points</param>
        /// <returns>routeSummary as List<RouteLogSequencedPointsDTO></returns>
        internal static List<RouteLogSummaryPoint> GetSequencedPointSummary(List<RouteLogSequencedPointsDTO> addressList)
        {
            // Initialize the list of summary points
            List<RouteLogSummaryPoint> summaryPoints = new List<RouteLogSummaryPoint>();

            // If there are any addresses to process
            if (addressList != null && addressList.Count > 0)
            {
                // Group
                // The group keeps track of the current group while the summary is being generated
                RouteLogSummaryGroup group = null;

                // Step through the addresses
                foreach (RouteLogSequencedPointsDTO address in addressList)
                {
                    // If the group is not set
                    if (group == null)
                    {
                        // The current address is the start of a new group
                        //
                        // Initialize a new group from the current address
                        group = new RouteLogSummaryGroup(address);
                    }
                    else
                    {
                        // If the current address can be added to the current group
                        if (Add(address, group))
                        {
                            // The address belongs to the current group
                            //
                            // Do nothing
                        }
                        else
                        {
                            // The address belongs to a new group
                            //
                            // Create a summary point from the current group
                            string specialInstructions = string.Empty; // TODO update when special instruction functionality is available
                            string hazardsAndAreaHazards = string.Empty; // TODO update when hazards and area hazards functionality is available
                            RouteLogSummaryPoint row = new RouteLogSummaryPoint(group.StreetName, group.Description, group.DeliveryPointCount.ToString(), group.MultipleOccupancy.ToString(), specialInstructions, hazardsAndAreaHazards);
                            summaryPoints.Add(row);

                            // Initialize a new group from the current address
                            group = new RouteLogSummaryGroup(address);
                        }
                    }
                }

                // If there is a group to process
                if (group != null)
                {
                    // Process the final group
                    //
                    // Create a summary point from the current group
                    string specialInstructions = string.Empty; // TODO update when special instruction functionality is available
                    string hazardsAndAreaHazards = string.Empty; // TODO update when hazards and area hazards functionality is available
                    RouteLogSummaryPoint row = new RouteLogSummaryPoint(group.StreetName, group.Description, group.DeliveryPointCount.ToString(), group.MultipleOccupancy.ToString(), specialInstructions, hazardsAndAreaHazards);
                    summaryPoints.Add(row);
                }
            }

            // Return the summary points
            return summaryPoints;
        }



        /// <summary>
        /// Determines whether the specified address continues the group
        /// To continue the group the address must:
        ///   Have the same thoroughfare
        /// AND
        ///   Continue the sequence predicted by the group type
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="group">The group</param>
        /// <returns>True if the address continues the group, otherwise false</returns>
        private static bool ContinuesGroup(RouteLogSequencedPointsDTO address, RouteLogSummaryGroup group)
        {
            // Assume that the address does not continue the group
            bool doesContinueGroup = false;

            // If the thoroughfare is the same as in the last address
            if (address.StreetName == group.LastAddress.StreetName)
            {
                // Determine whether the group type for the address is the same as the group type
                GroupType groupTypeForAddress = IdentifyGroupType(address, group);
                if (groupTypeForAddress == group.CurrentGroupType)
                {
                    doesContinueGroup = true;
                }
            }

            // Return whether the address continues the group
            return doesContinueGroup;
        }



        /// <summary>
        /// Gets the increment (absolute difference) between two numbers
        /// </summary>
        /// <param name="number1">Number 1</param>
        /// <param name="number2">Number 2</param>
        /// <returns>The increment</returns>
        private static short GetIncrement(short number1, short number2)
        {
            // The increment is the absolute difference between the two numbers
            return System.Math.Abs((short)(number1 - number2));
        }



        /// <summary>
        /// Determines whether a specified address has a standard building number
        /// A standard building number is a simple non-zero, positive integer building number
        ///   without a building name or sub name
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>True if the address has a standard building number, otherwise false</returns>
        private static bool HasStandardBuildingNumber(RouteLogSequencedPointsDTO address)
        {
            // The building number is standard if it is a positive non-zero value
            bool isStandard = false;
            if (address.BuildingNumber.HasValue)
            {
                if (address.BuildingNumber.Value > 0)
                {
                    isStandard = true;
                }
            }

            // If the building number is standard
            if (isStandard)
            {
                // If the building also has a name or a sub name
                if (!string.IsNullOrWhiteSpace(address.SubBuildingName) || !string.IsNullOrWhiteSpace(address.BuildingName))
                {
                    // The building number is not standard
                    isStandard = false;
                }
            }

            // Return whether the address has a standard building number
            return isStandard;
        }



        /// <summary>
        /// Identifies the group type using the address and the last address in the group
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="group">The group</param>
        /// <returns>The group type</returns>
        private static GroupType IdentifyGroupType(RouteLogSequencedPointsDTO address, RouteLogSummaryGroup group)
        {
            // Assume that the group type is unknown
            GroupType groupType = GroupType.Unknown;

            // Determine the group type
            //
            // If the last address and address has a standard building number
            if (HasStandardBuildingNumber(group.LastAddress) && HasStandardBuildingNumber(address))
            {
                // Determine the group type from the building numbers
                groupType = IdentifyGroupType(group.LastAddress.BuildingNumber.Value, address.BuildingNumber.Value);
            }
            else
            {
                // Only addresses with standard building numbers can be matched to a group type
                groupType = GroupType.Unknown;
            }

            // Return the group type
            return groupType;
        }



        /// <summary>
        /// Identifies the group type for two building numbers
        /// </summary>
        /// <param name="buildingNumber1">Building number 1</param>
        /// <param name="buildingNumber2">Building number 2</param>
        /// <returns>The group type</returns>
        private static GroupType IdentifyGroupType(short buildingNumber1, short buildingNumber2)
        {
            // Assume that the group type is unknown
            GroupType groupType = GroupType.Unknown;

            // Get the metadata for the building numbers
            bool isAscending = buildingNumber1 < buildingNumber2;
            short increment = GetIncrement(buildingNumber1, buildingNumber2);
            bool areEven = IsEven(buildingNumber1) && IsEven(buildingNumber2);
            bool areOdd = !IsEven(buildingNumber1) && !IsEven(buildingNumber2);

            // Identify the group type based on the building number metadata
            //
            // If the building numbers are in ascending order
            if (isAscending)
            {
                // If the building numbers are sequential integers
                if (increment == 1)
                {
                    groupType = GroupType.ConsecutiveAscending;
                }
                else
                {
                    // If the building numbers are both even and sequential
                    if (areEven && increment == 2)
                    {
                        groupType = GroupType.EvensAscending;
                    }

                    // If the building numbers are both odd and sequential
                    if (areOdd && increment == 2)
                    {
                        groupType = GroupType.OddsAscending;
                    }
                }
            }
            else
            {
                // The numbers are in descending order
                //
                // If the building numbers are sequential integers
                if (increment == 1)
                {
                    groupType = GroupType.ConsecutiveDescending;
                }
                else
                {
                    // If the building numbers are both even and sequential
                    if (areEven && increment == 2)
                    {
                        groupType = GroupType.EvensDescending;
                    }

                    // If the building numbers are both odd and sequential
                    if (areOdd && increment == 2)
                    {
                        groupType = GroupType.OddsDescending;
                    }
                }
            }

            // Return the group type
            return groupType;
        }



        /// <summary>
        /// Determines whether the specified number is even
        /// </summary>
        /// <param name="number">The number</param>
        /// <returns>True if the number is even, otherwise false</returns>
        private static bool IsEven(short number)
        {
            // The number is even if remainder when dividing by two is zero
            return number % 2 == 0;
        }
    }
}
