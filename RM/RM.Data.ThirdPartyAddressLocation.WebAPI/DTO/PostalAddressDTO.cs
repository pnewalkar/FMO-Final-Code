using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RM.CommonLibrary.EntityFramework.DTO.UIDropdowns;

namespace RM.Data.ThirdPartyAddressLocation.WebAPI.DTO
{
    public class PostalAddressDTO
    {
        /// <summary>
        /// This class represents data transfer object for PostalAddress entity
        /// </summary>
        public string PostcodeType { get; set; }

        [StringLength(60)]
        public string OrganisationName { get; set; }

        [StringLength(60)]
        public string DepartmentName { get; set; }

        [StringLength(50)]
        public string BuildingName { get; set; }

        public short? BuildingNumber { get; set; }

        [StringLength(50)]
        public string SubBuildingName { get; set; }

        [StringLength(80)]
        public string Thoroughfare { get; set; }

        [StringLength(80)]
        public string DependentThoroughfare { get; set; }

        [StringLength(35)]
        public string DependentLocality { get; set; }

        [StringLength(35)]
        public string DoubleDependentLocality { get; set; }

        [StringLength(30)]
        public string PostTown { get; set; }

        [StringLength(8)]
        public string Postcode { get; set; }

        [StringLength(2)]
        public string DeliveryPointSuffix { get; set; }

        [StringLength(1)]
        public string SmallUserOrganisationIndicator { get; set; }

        public int? UDPRN { get; set; }

        public bool? AMUApproved { get; set; }

        [StringLength(6)]
        public string POBoxNumber { get; set; }

        public Guid ID { get; set; }

        public Guid PostCodeGUID { get; set; }

        public Guid AddressType_GUID { get; set; }

        public Guid? AddressStatus_GUID { get; set; }

        public bool IsValidData { get; set; }

        public string InValidRemarks { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string AmendmentType { get; set; }

        public string AmendmentDesc { get; set; }

        public string FileName { get; set; }
    }
}