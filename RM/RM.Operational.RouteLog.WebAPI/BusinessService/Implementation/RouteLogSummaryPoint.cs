using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    /// <summary>
    /// Contains information about a single row in the sequenced points or unsequenced points sections
    ///   of the route log summary
    /// The fields are all string data types because the data in this class should be formatted for display
    /// Note that the number field contains information about the building number or numbers (for a 
    ///   summary of a sequence of points)
    /// </summary>
    internal class RouteLogSummaryPoint
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="street">The street</param>
        /// <param name="number">The number</param>
        /// <param name="dps">Delivery points</param>
        /// <param name="multipleOccupancy">Multiple occupancy</param>
        /// <param name="specialInstructions">Special instructions</param>
        /// <param name="hazardsAndAreaHazards">Hazards and area hazards</param>
        public RouteLogSummaryPoint(string street, string number, string dps, string multipleOccupancy, string specialInstructions, string hazardsAndAreaHazards)
        {
            this.Street = street;
            this.Number = number;
            this.DPs = dps;
            this.MultipleOccupancy = multipleOccupancy;
            this.SpecialInstructions = specialInstructions;
            this.HazardsAndAreaHazards = hazardsAndAreaHazards;
        }





        /// <summary>
        /// Gets or sets the number of the summary point
        /// </summary>
        public string Number { get; set; }



        /// <summary>
        /// Gets or sets the number of delivery points (as a string)
        /// </summary>
        public string DPs { get; set; }



        /// <summary>
        /// Gets or sets the hazards and area hazards
        /// </summary>
        public string HazardsAndAreaHazards { get; set; }



        /// <summary>
        /// Gets or sets the multiple occupancy count (as a string)
        /// </summary>
        public string MultipleOccupancy { get; set; }



        /// <summary>
        /// Gets or sets the special instructions
        /// </summary>
        public string SpecialInstructions { get; set; }



        /// <summary>
        /// Gets or sets the street
        /// </summary>
        public string Street { get; set; }
    }
}
