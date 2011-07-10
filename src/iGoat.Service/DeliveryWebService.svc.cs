using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using iGoat.Domain;
using iGoat.Domain.Entities;
using iGoat.Service.Contracts;
using DeliveryItemStatus = iGoat.Service.Contracts.DeliveryItemStatus;

namespace iGoat.Service
{
    public class DeliveryWebService : IDeliveryWebService
    {
        private readonly IProfileService _profileService;

        public DeliveryWebService(IProfileService profileService)
        {
            _profileService = profileService;
        }

        #region IDeliveryWebService Members

        public SuccessfulLoginResponse Login(string username, string password)
        {
            try
            {
                string authKey = _profileService.GetAuthKey(username, password);
                return new SuccessfulLoginResponse
                           {
                               AuthKey = authKey
                           };
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new FaultException<UnauthorizedAccessException>(ex);
            }
        }

        public List<DeliveryItemSummary> GetMyItems(string authKey, DeliveryItemStatus deliveryItemStatus)
        {
            Profile profile = _profileService.GetProfile(authKey);
            return profile.Items
                .Where(x => x.Status == (Domain.DeliveryItemStatus) deliveryItemStatus)
                .Select(x => new DeliveryItemSummary
                                 {
                                     Id = x.Id,
                                     Type = x.ItemType.Name,
                                 }).ToList();
        }

        public List<DeliverySummary> GetMyDeliveries(string authKey)
        {
            Profile profile = _profileService.GetProfile(authKey);
            return profile.Deliveries.Select(x => new DeliverySummary
                                                      {
                                                          Id = x.Id,
                                                          ItemCount = x.Items.Count,
                                                          CompletedOn = x.CompletedOn,
                                                      }).ToList();            
        }

        public DeliveryDetails GetDeliveryDetails(string authKey, int deliveryId)
        {
            Profile profile = _profileService.GetProfile(authKey);
            var delivery = profile.Deliveries.SingleOrDefault(x => x.Id == deliveryId);
            return new DeliveryDetails
                       {
                           Id = delivery.Id,
                           CompletedOn = delivery.CompletedOn,
                           Location = new LocationData
                                          {
                                              Name = delivery.Location.Name,
                                              Latitude = delivery.Location.Latitude,
                                              Longitude = delivery.Location.Longitue,
                                          },
                           Items = delivery.Items.Select(x => new DeliveryItemSummary
                                                                  {
                                                                      Id = x.Id,
                                                                      Type = x.ItemType.ToString(),
                                                                  }).ToList()
                       };
        }

        public DeliveryItemDetails GetDeliveryItemDetails(string authKey, int deliveryItemId)
        {
            Profile profile = _profileService.GetProfile(authKey);
            var deliveryItem = profile.Items.SingleOrDefault(x => x.Id == deliveryItemId);
            return new DeliveryItemDetails
                       {
                           Id = deliveryItem.Id,
                           Status = deliveryItem.Status.ToString(),
                           Type = deliveryItem.ItemType.Name,
                       };
        }

        #endregion
    }
}