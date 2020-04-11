using JICtravel.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JICtravel.Common.Service
{
    public interface IApiService
    {
        Task<Response> GetTripAsync(string document, string urlBase, string servicePrefix, string controller);
    }
}
