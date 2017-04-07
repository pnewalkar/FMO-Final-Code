using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using System.IO;
using Newtonsoft.Json;
using Fmo.Helpers.Interface;

namespace Fmo.BusinessServices.Services
{
    public class AccessLinkBussinessService : IAccessLinkBussinessService
    {
        private IAccessLinkRepository accessLinkRepository = default(IAccessLinkRepository);

        private ICreateOtherLayersObjects createOtherLayersObjects = default(ICreateOtherLayersObjects);

        public AccessLinkBussinessService(IAccessLinkRepository searchAccessLinkRepository, ICreateOtherLayersObjects createOtherLayerObjects)
        {
            this.accessLinkRepository = searchAccessLinkRepository;
            this.createOtherLayersObjects = createOtherLayerObjects;
        }

        public List<AccessLinkDTO> SearchAccessLink()
        {
            return accessLinkRepository.SearchAccessLink();
        }

        public MemoryStream GetAccessLinks(string bbox)
        {
            string[] bboxArr = bbox.Split(',');
            var coordinates = GetData(null, bboxArr);
            AccessLinkDTO accessLinkDTOCollectionObj = new AccessLinkDTO();
            List<AccessLinkDTO> AccessLinkDTO = accessLinkRepository.GetAccessLinks(coordinates);

            List<Feature> lstFeatures = new List<Feature>();

            string json = string.Empty;

            foreach (var res in AccessLinkDTO)
            {
                Geometry geometry = new Geometry();

                geometry.type = res.AccessLinkLine.SpatialTypeName;

                var resultCoordinates = res.AccessLinkLine;

                geometry.coordinates = new object();

                Feature features = createOtherLayersObjects.getAccessLinks(geometry, resultCoordinates);
                lstFeatures.Add(features);
            }
            accessLinkDTOCollectionObj.features = lstFeatures;
            accessLinkDTOCollectionObj.type = "FeatureCollection";
            json = JsonConvert.SerializeObject(
                accessLinkDTOCollectionObj,
                            Newtonsoft.Json.Formatting.None,
                            new
                            JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            var resultBytes = System.Text.Encoding.UTF8.GetBytes(json);

            return new MemoryStream(resultBytes);
        }

        public string GetData(string query, params object[] parameters)
        {
            string x1 = Convert.ToString(parameters[0]);
            string y1 = Convert.ToString(parameters[1]);
            string x2 = Convert.ToString(parameters[2]);
            string y2 = Convert.ToString(parameters[3]);
            string coordinates = "POLYGON((" + x1 + " " + y1 + ", " + x1 + " " + y2 + ", " + x2 + " " + y2 + ", " + x2 + " " + y1 + ", " + x1 + " " + y1 + "))";

            return coordinates;
        }
    }
}
