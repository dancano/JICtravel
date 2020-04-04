using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JICtravel.Web.Data;
using JICtravel.Web.Data.Entities;
using JICtravel.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JICtravel.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlavesController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public SlavesController(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        // GET: api/Trips
        [HttpGet]
        public IEnumerable<SlaveEntity> GetTrips()
        {
            return _context.Slaves;
        }

        [HttpGet("{document}")]
        public async Task<IActionResult> GetSlaveEntity([FromRoute] string document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SlaveEntity slaveEntity = await _context.Slaves
                .Include(s => s.Trips)
                .ThenInclude(s => s.TripDetails)
                .ThenInclude(s => s.ExpensivesType)
                .FirstOrDefaultAsync(t => t.Document == document);

            if (slaveEntity == null)
            {

            }

            return Ok(_converterHelper.ToSlaveResponse(slaveEntity));
        }

    }
}