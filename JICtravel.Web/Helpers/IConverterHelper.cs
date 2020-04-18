using JICtravel.Common.Models;
using JICtravel.Web.Data.Entities;
using System.Collections.Generic;

namespace JICtravel.Web.Helpers
{
    public interface IConverterHelper
    {
        SlaveResponse ToSlaveResponse(SlaveEntity slaveEntity);

        TripResponse ToTripResponse(TripEntity tripEntity);

        List<TripResponse> ToTripResponse(List<TripEntity> tripEntity);

    }

}
