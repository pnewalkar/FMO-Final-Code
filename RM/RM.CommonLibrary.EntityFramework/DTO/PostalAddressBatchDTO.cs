﻿using System;
using System.Collections.Generic;
using RM.CommonLibrary.EntityFramework.DTO.UIDropdowns;

namespace RM.CommonLibrary.EntityFramework.DTO
{
    public class PostalAddressBatchDTO
    {
        /// <summary>
        /// This class represents data transfer object for PostalAddress entity
        /// </summary>
        public string PostcodeType { get; set; }

        public string OrganisationName { get; set; }

        public string DepartmentName { get; set; }

        public string BuildingName { get; set; }

        public short? BuildingNumber { get; set; }

        public string SubBuildingName { get; set; }

        public string Thoroughfare { get; set; }

        public string DependentThoroughfare { get; set; }

        public string DependentLocality { get; set; }

        public string DoubleDependentLocality { get; set; }

        public string PostTown { get; set; }

        public string Postcode { get; set; }

        public string DeliveryPointSuffix { get; set; }

        public string SmallUserOrganisationIndicator { get; set; }

        public int? UDPRN { get; set; }

        public bool? AMUApproved { get; set; }

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