using JICtravel.Common.Models;
using JICtravel.Web.Data.Entities;

namespace JICtravel.Web.Helpers
{
    public interface IConverterHelper
    {
        SlaveResponse ToSlaveResponse(SlaveEntity slaveEntity);
    }

}
