using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace iGoat.Service
{
    [ServiceContract]
    public interface IDeliveryWebService
    {
        [OperationContract]
        SuccessfulLoginResponse Login(string username, string password);

        [OperationContract]
        List<DeliveryItemSummary> GetMyItems(string authKey, DeliveryItemStatus deliveryItemStatus);

        [OperationContract]
        List<DeliverySummary> GetMyDeliveries(string authKey);

        [OperationContract]
        DeliveryData GetDeliveryDetails(string authKey, int deliveryId);
    }
}
