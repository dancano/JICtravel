using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JICtravel.Common.Models;
using JICtravel.Web.Data.Entities;

namespace JICtravel.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public SlaveResponse ToSlaveResponse(SlaveEntity slaveEntity)
        {
            return new SlaveResponse
            {
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
                        PicturePath = td.PicturePath,
                        expensiveTypes = td.ExpensivesType?.Select(et => new ExpensiveTypeResponse
                        {
                            Id = et.Id,
                            ExpensiveType = et.ExpensiveType
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
        }


    }
}