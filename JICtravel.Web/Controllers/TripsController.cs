﻿using JICtravel.Common.Enums;
using JICtravel.Web.Data;
using JICtravel.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JICtravel.Web.Controllers
{   
    [Authorize(Roles = "Admin")]
    public class TripsController : Controller
    {
        private readonly DataContext _context;

        public TripsController(DataContext context)
        {
            _context = context;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Where(u => u.UserType == UserType.Slave)
                .Include(u => u.Trips)
                .OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
                .ToListAsync());
        }

        // GET: Trips/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SlaveEntity slaveEntity = await _context.Users
                .Include(d => d.Trips)
                .ThenInclude(d => d.TripDetails)
                .FirstOrDefaultAsync(d => d.Document == id);
            if (slaveEntity == null)
            {
                return NotFound();
            }

            return View(slaveEntity);
        }

        public async Task<IActionResult> ExpenseDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TripEntity tripEntity = await _context.Trips
                .Include(d => d.TripDetails)
                .ThenInclude(d => d.ExpensivesType)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (tripEntity == null)
            {
                return NotFound();
            }

            return View(tripEntity);
        }

    }
}
