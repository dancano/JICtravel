using JICtravel.Common.Models;
using JICtravel.Web.Data;
using JICtravel.Web.Data.Entities;
using JICtravel.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JICtravel.Web.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;

        public TripsController(
            DataContext context,
            IImageHelper imageHelper,
            IUserHelper userHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }

        [HttpPost]
        [Route("GetUserTrips")]
        public async Task<IActionResult> GetUserTrips([FromBody] TripRequest tripRequest)
        {
            SlaveEntity slaveEntity = await _userHelper.GetUserAsync(tripRequest.UserId);
            if (slaveEntity == null)
            {
                return BadRequest("User doesn't exists.");
            }

            List<TripEntity> tripsUser = await _context.Trips
                .Include(t => t.TripDetails)
                .ThenInclude(t => t.ExpensiveType)
                .Where(t => t.Slave.Id == tripRequest.UserId.ToString())
                .ToListAsync();

            return Ok(_converterHelper.ToTripResponse(tripsUser));
        }


        [HttpPost]
        public async Task<IActionResult> PostTripEntity([FromBody] TripRequest tripRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SlaveEntity slaveEntity = await _userHelper.GetUserAsync(tripRequest.UserId);
            if (slaveEntity == null)
            {
                return BadRequest("User doesn't exists.");
            }

            TripEntity tripEntity = new TripEntity
            {
                CityVisited = tripRequest.CityVisited,
                StartDate = tripRequest.StartDate,
                EndDate = tripRequest.EndDate,
                Slave = slaveEntity,
            };

            _context.Trips.Add(tripEntity);
            await _context.SaveChangesAsync();
            return Ok(_converterHelper.ToTripResponse(tripEntity));
        }

        [HttpPost]
        [Route("GetExpensesTrip")]
        public async Task<IActionResult> GetExpensesTrip([FromBody] TripDetailRequest tripDetailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<TripEntity> ExpenseTripUser = await _context.Trips
                .Include(t => t.TripDetails)
                .ThenInclude(t => t.ExpensiveType)
                .Where(t => t.Id == tripDetailRequest.TripId)
                .ToListAsync();

            return Ok(_converterHelper.ToTripResponse(ExpenseTripUser));
        }

        [HttpPost]
        [Route("PostExpensesTrip")]
        public async Task<IActionResult> PostExpensesTrip([FromBody] TripDetailRequest tripDetailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TripEntity tripEntity = await _context.Trips
                .FirstOrDefaultAsync(d => d.Id == tripDetailRequest.TripId);

            ExpensiveTypeEntity expensiveType = await _context.ExpensivesType
                .FirstOrDefaultAsync(d => d.Id == tripDetailRequest.ExpensiveTypeId);

            string picturePathExpense = string.Empty;
            if (tripDetailRequest.PictureArrayExpense != null && tripDetailRequest.PictureArrayExpense.Length > 0)
            {
                picturePathExpense = _imageHelper.UploadImage(tripDetailRequest.PictureArrayExpense, "Invoice");
            }

            TripDetailEntity tripDetailEntity = new TripDetailEntity
            {
                StartDate = tripDetailRequest.StartDate,
                Expensive = tripDetailRequest.Expensive,
                Trips = tripEntity,
                ExpensiveType = expensiveType,
                PicturePathExpense = picturePathExpense
            };

            _context.TripDetails.Add(tripDetailEntity);
            await _context.SaveChangesAsync();
           return Ok(_converterHelper.ToExpenseResponse(tripDetailEntity));
        }
    }
}