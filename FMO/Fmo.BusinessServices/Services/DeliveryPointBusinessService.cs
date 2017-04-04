using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using System.Net.Http;
using System.IO;
using Fmo.DataServices.Repositories;
using System.Net;
using System.Net.Http.Headers;

namespace Fmo.BusinessServices.Services
{
    public class DeliveryPointBusinessService : IDeliveryPointBusinessService
    {
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);

        public DeliveryPointBusinessService(IDeliveryPointsRepository deliveryPointsRepository)
        {
            this.deliveryPointsRepository = deliveryPointsRepository;
        }

        public HttpResponseMessage GetDeliveryPoints()
        {
            //object[] bboxArr = bbox.Split(',');
            //return searchDeliveryPointsRepository.GetDeliveryPoints(bboxArr);



            MemoryStream deliveryPoints = this.searchDeliveryPointsRepository.GetDeliveryPoints();

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(deliveryPoints)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            result.Content.Headers.ContentLength = deliveryPoints.Length;
            return result;

        }
    }
}
