using JICtravel.Common.Models;
using System.Threading.Tasks;

namespace JICtravel.Common.Service
{
    public interface IApiService
    {
        Task<Response> GetTripAsync(string document, string urlBase, string servicePrefix, string controller);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<Response> GetUserByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, EmailRequest request);

        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, SlaveRequest slaveRequest);

    }
}
