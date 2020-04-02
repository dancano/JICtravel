using JICtravel.Common.Enums;
using JICtravel.Web.Data;
using JICtravel.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JICtravel.Web.Controllers
{
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
                .FirstOrDefaultAsync(d => d.Document == id);
            if (slaveEntity == null)
            {
                return NotFound();
            }

            return View(slaveEntity);
        }


    }
}
