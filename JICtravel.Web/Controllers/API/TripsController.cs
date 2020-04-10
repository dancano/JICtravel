using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JICtravel.Web.Data;
using JICtravel.Web.Data.Entities;
using JICtravel.Web.Helpers;

namespace JICtravel.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public TripsController(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        // GET: api/Trips
        [HttpGet]
        public IEnumerable<TripEntity> GetTrips()
        {
            return _context.Trips;
        }

        // GET: api/Trips/5 CONSULTAR
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripEntity([FromRoute] int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TripEntity tripEntity = await _context.Trips
                .Include(t => t.Slave)
                .Include(t => t.TripDetails)
                .ThenInclude(t => t.ExpensiveType)
                .Include(t => t.Slave)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tripEntity == null)
            {

            }

            return Ok(tripEntity);
        }

        // POST: api/Trips CREAR
        [HttpPost]
        public async Task<IActionResult> PostTripEntity([FromBody] TripEntity tripEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Trips.Add(tripEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripEntity", new { id = tripEntity.Id }, tripEntity);
        }


        private bool TripEntityExists(int id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}