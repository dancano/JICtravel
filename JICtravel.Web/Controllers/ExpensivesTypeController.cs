using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JICtravel.Web.Data;
using JICtravel.Web.Data.Entities;

namespace JICtravel.Web.Controllers
{
    public class ExpensivesTypeController : Controller
    {
        private readonly DataContext _context;

        public ExpensivesTypeController(DataContext context)
        {
            _context = context;
        }

        // GET: ExpensivesType
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExpensivesType.ToListAsync());
        }

        // GET: ExpensivesType/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpensiveTypeEntity expensiveTypeEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expensiveTypeEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expensiveTypeEntity);
        }

        // GET: ExpensivesType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expensiveTypeEntity = await _context.ExpensivesType.FindAsync(id);
            if (expensiveTypeEntity == null)
            {
                return NotFound();
            }
            return View(expensiveTypeEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExpensiveTypeEntity expensiveTypeEntity)
        {
            if (id != expensiveTypeEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(expensiveTypeEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(expensiveTypeEntity);
        }

        // GET: ExpensivesType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expensiveTypeEntity = await _context.ExpensivesType.FirstOrDefaultAsync(m => m.Id == id);

            _context.ExpensivesType.Remove(expensiveTypeEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
