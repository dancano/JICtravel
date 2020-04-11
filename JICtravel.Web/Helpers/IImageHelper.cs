using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace JICtravel.Web.Helpers
{
    public interface IImageHelper
    {
        Task<string> UpLoadImageAsync(IFormFile imageFile, string folder);
    }
}
