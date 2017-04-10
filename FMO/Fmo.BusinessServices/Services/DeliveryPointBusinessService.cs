namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Script.Serialization;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Helpers;
    using Microsoft.SqlServer.Types;
    using Newtonsoft.Json.Linq;

    public class DeliveryPointBusinessService : IDeliveryPointBusinessService
    {
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);

        public DeliveryPointBusinessService(IDeliveryPointsRepository deliveryPointsRepository)
        {
            this.deliveryPointsRepository = deliveryPointsRepository;
        }

        public object GetDeliveryPoints()
        {
            //object[] bboxArr = bbox.Split(',');
            //return searchDeliveryPointsRepository.GetDeliveryPoints(bboxArr);

            string str = File.ReadAllText(@"D:\Richa\FMO-AD\FMO\Fmo.DataServices\TestData\deliveryPoint.json");
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize(str, typeof(object));

            //MemoryStream deliveryPoints = this.deliveryPointsRepository.GetDeliveryPoints();

            //var result = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new StreamContent(deliveryPoints)
            //};

            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //result.Content.Headers.ContentLength = deliveryPoints.Length;
            //return result;

        }

        public DeliveryPointDTO GetDeliveryPoints1(string bbox)
        {
            try
            {
                object[] bboxArr = bbox.Split(',');
                var coordinates = GetData(null, bboxArr);
                List<DeliveryPointDTO> lst = deliveryPointsRepository.GetDeliveryPoints1(coordinates);
                DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO();
                var geoJson = new GeoJson
                {
                    features = new List<Feature>()
                };

                foreach (var point in lst)
                {
                    SqlGeometry sqlGeo = SqlGeometry.STGeomFromWKB(new SqlBytes(point.LocationXY.AsBinary()), 0);

                    var feature = new Feature
                    {
                        id = point.DeliveryPoint_Id,
                        properties = new Dictionary<string, JToken>
                    {
                        { "name", point.PostalAddress.BuildingName },
                        { "number", point.PostalAddress.BuildingNumber },
                        { "postcode", point.PostalAddress.Postcode },
                        { "street_name", point.PostalAddress.BuildingName },
                        { "type", "deliverypoint" }
                    },
                        geometry = new Geometry
                        {
                            coordinates = new Coordinates(sqlGeo)
                        }
                    };
                    geoJson.features.Add(feature);
                }

                var resultBytes = System.Text.Encoding.UTF8.GetBytes(geoJson.getJson().ToString());

                MemoryStream mStream = new MemoryStream(resultBytes);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(mStream)
                };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                result.Content.Headers.ContentLength = mStream.Length;
                return deliveryPointDTO;
            }
            catch (Exception)
            {
                throw;
            }
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
