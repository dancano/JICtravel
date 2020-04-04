using JICtravel.Common.Models;
using JICtravel.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JICtravel.Web.Helpers
{
    public interface IConverterHelper
    {
        SlaveResponse ToSlaveResponse(SlaveEntity slaveEntity);
    }

}
