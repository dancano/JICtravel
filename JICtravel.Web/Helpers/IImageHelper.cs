using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JICtravel.Web.Helpers
{
    public interface IImageHelper
    {
        Task<String> UpLoadImageAsync(IFormFile imageFile, string folder);
    }
}
