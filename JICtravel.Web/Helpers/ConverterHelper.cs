using JICtravel.Common.Models;
using JICtravel.Web.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace JICtravel.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public SlaveResponse ToSlaveResponse(SlaveEntity slaveEntity)
        {
            return new SlaveResponse
            {   
                Id = slaveEntity.Id,
                Document = slaveEntity.Document,
                FirstName = slaveEntity.FirstName,
                LastName = slaveEntity.LastName,
                Email = slaveEntity.Email,
                PicturePath = slaveEntity.PicturePath,
                Trips = slaveEntity.Trips?.Select(t => new TripResponse
                {
                    Id = t.Id,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    CityVisited = t.CityVisited,
                    tripDetails = t.TripDetails?.Select(td => new TripDetailResponse
                    {
                        Id = td.Id,
                        StartDate = td.StartDate,
                        Expensive = td.Expensive,
                        PicturePath = td.PicturePath
                    }).ToList()
                }).ToList()
            };
        }

        public TripResponse ToTripResponse(TripEntity tripEntity)
        {
            return new TripResponse
            {
                Id = tripEntity.Id,
                StartDate = tripEntity.StartDate,
                EndDate = tripEntity.EndDate,
                CityVisited = tripEntity.CityVisited,
                tripDetails = tripEntity.TripDetails?.Select(td => new TripDetailResponse
                {
                    Id = td.Id,
                    StartDate = td.StartDate,
                    Expensive = td.Expensive,
                    PicturePath = td.PicturePath
                }).ToList(),
            };
        }

        public List<TripResponse> ToTripResponse(List<TripEntity> tripEntities)
        {
            return tripEntities.Select(t => new TripResponse
            {
                Id = t.Id,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                CityVisited = t.CityVisited,
                tripDetails = t.TripDetails.Select(td => new TripDetailResponse
                {
                    StartDate = td.StartDate,
                    Expensive = td.Expensive,
                    PicturePath = td.PicturePath
                }).ToList()
            }).ToList();
        }
    }
}