﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using JICtravel.Common.Enums;
using JICtravel.Common.Models;
using JICtravel.Web.Data;
using JICtravel.Web.Data.Entities;
using JICtravel.Web.Helpers;
using System.Globalization;
using JICtravel.Web.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace JICtravel.Web.Controllers.API
{
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AccountController(
            DataContext dataContext,
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            SlaveEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user != null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = Resource.UserAlreadyExists
                });
            }

            string picturePath = string.Empty;
            if (request.PictureArray != null && request.PictureArray.Length > 0)
            {
                picturePath = _imageHelper.UploadImage(request.PictureArray, "Users");
            }

            user = new SlaveEntity
            {
                Document = request.Document,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                PicturePath = picturePath,
                UserType = request.UserTypeId == 1 ? UserType.Slave : UserType.Admin
            };

            IdentityResult result = await _userHelper.AddUserAsync(user, request.Password);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }

            SlaveEntity userNew = await _userHelper.GetUserAsync(request.Email);
            await _userHelper.AddUserToRoleAsync(userNew, user.UserType.ToString());

            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendMail(request.Email, Resource.EmailConfirmationSubject, $"<h1>{Resource.EmailConfirmationSubject}</h1>" +
                $"{Resource.EmailConfirmationBody}</br></br><a href = \"{tokenLink}\">{Resource.ConfirmEmail}</a>");

            return Ok(new Response
            {
                IsSuccess = true,
                Message = Resource.EmailConfirmationSent
            });
        }

        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            SlaveEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = Resource.UserNotFoundError
                });
            }

            string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
            _mailHelper.SendMail(request.Email, Resource.RecoverPasswordSubject, $"<h1>{Resource.RecoverPasswordSubject}</h1>" +
                $"{Resource.RecoverPasswordBody}</br></br><a href = \"{link}\">{Resource.RecoverPasswordSubject}</a>");

            return Ok(new Response
            {
                IsSuccess = true,
                Message = Resource.RecoverPasswordEmailSent
            });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            SlaveEntity userEntity = await _userHelper.GetUserAsync(request.Email);
            if (userEntity == null)
            {
                return BadRequest(Resource.UserNotFoundError);
            }

            string picturePath = userEntity.PicturePath;
            if (request.PictureArray != null && request.PictureArray.Length > 0)
            {
                picturePath = _imageHelper.UploadImage(request.PictureArray, "Users");
            }

            userEntity.Document = request.Document;
            userEntity.FirstName = request.FirstName;
            userEntity.LastName = request.LastName;
            userEntity.PicturePath = picturePath;

            IdentityResult respose = await _userHelper.UpdateUserAsync(userEntity);
            if (!respose.Succeeded)
            {
                return BadRequest(respose.Errors.FirstOrDefault().Description);
            }

            SlaveEntity updatedUser = await _userHelper.GetUserAsync(request.Email);
            return Ok(updatedUser);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            CultureInfo cultureInfo = new CultureInfo(request.CultureInfo);
            Resource.Culture = cultureInfo;

            SlaveEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = Resource.UserNotFoundError
                });
            }

            IdentityResult result = await _userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = result.Errors.FirstOrDefault().Description
                });
            }

            return Ok(new Response
            {
                IsSuccess = true,
                Message = Resource.ChangePasswordSuccess
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromBody] EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            CultureInfo cultureInfo = new CultureInfo(emailRequest.CultureInfo);
            Resource.Culture = cultureInfo;

            SlaveEntity userEntity = await _userHelper.GetUserAsync(emailRequest.Email);
            if (userEntity == null)
            {
                return NotFound(Resource.UserNotFoundError);
            }

            return Ok(_converterHelper.ToSlaveResponse(userEntity));
        }


    }

}