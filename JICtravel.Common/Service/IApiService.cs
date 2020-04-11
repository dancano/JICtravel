using JICtravel.Common.Models;
using System.Threading.Tasks;

namespace JICtravel.Common.Service
{
    public interface IApiService
    {
        Task<Response> GetTripAsync(string document, string urlBase, string servicePrefix, string controller);

        Task<bool> CheckConnectionAsync(string url);
    }
}
