using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using iGoat.Domain;
using iGoat.Domain.Entities;
using iGoat.Service.Contracts;
using DeliveryItemStatus = iGoat.Service.Contracts.DeliveryItemStatus;
using Profile = iGoat.Domain.Entities.Profile;

namespace iGoat.Service
{
    public class DeliveryWebService : IDeliveryWebService
    {
        private readonly IProfileService _profileService;
        private readonly IEventProcessorFactory _eventProcessorFactory;
        private readonly IMappingEngine _mappingEngine;

        public DeliveryWebService(IProfileService profileService, IEventProcessorFactory eventProcessorFactory, IMappingEngine mappingEngine)
        {
            _profileService = profileService;
            _eventProcessorFactory = eventProcessorFactory;
            _mappingEngine = mappingEngine;
        }

        #region IDeliveryWebService Members

        public SuccessfulLoginResponse Login(string username, string password)
        {
            try
            {
                var authKey = _profileService.GetAuthKey(username, password);
                return new SuccessfulLoginResponse
                           {
                               AuthKey = authKey.AuthKey,
                               Expires = authKey.Expires,
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
            if (delivery == null)
                throw new FaultException("Delivery not found.");

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
            if (deliveryItem == null)
                throw new FaultException("Delivery item not found.");

            return new DeliveryItemDetails
                       {
                           Id = deliveryItem.Id,
                           Status = deliveryItem.Status.ToString(),
                           Type = deliveryItem.ItemType.Name,
                       };
        }

        public void ProcessEvent(IProcessEventRequest processEventRequest)
        {
            var @event = _mappingEngine.Map<IProcessEventRequest, IEvent>(processEventRequest);
            _eventProcessorFactory.Create(@event).Process(@event);
        }

        #endregion
    }
}