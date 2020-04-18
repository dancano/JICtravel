using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using JICtravel.Common.Enums;
using JICtravel.Common.Models;
using JICtravel.Web.Data;
using JICtravel.Web.Data.Entities;
using JICtravel.Web.Helpers;
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

        public AccountController(
            DataContext dataContext,
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _imageHelper = imageHelper;
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] SlaveRequest request)
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

            SlaveEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user != null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "User exist"
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
                UserType = request.UserTypeId == 1 ? UserType.Admin : UserType.Slave
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

            _mailHelper.SendMail(request.Email, "Email Confimation", $"<h1>Email</h1>" +
                $"Welcome to Just In Case Travels</br></br><a href = \"{tokenLink}\"> Please clic Here for Confirm Email</a>");

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "Send message for confirm Email"
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

            SlaveEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Email don't exists"
                });
            }

            string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
            _mailHelper.SendMail(request.Email, "Recover your password", $"<h1>Recover Password</h1>" +
                $"For recover your pass clic here</br></br><a href = \"{link}\"> Please Hurry Up</a>");

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "Recover Success"
            });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUser([FromBody] SlaveRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SlaveEntity userEntity = await _userHelper.GetUserAsync(request.Email);
            if (userEntity == null)
            {
                return BadRequest("User don't exists");
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

            SlaveEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "User not found"
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
                Message = "Change Password Success"
            });
        }

    }
}