using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotNETCoreRESTAPI.Data;
using dotNETCoreRESTAPI.Models;
using dotNETCoreRESTAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using dotNETCoreRESTAPI.Extensions;

namespace dotNETCoreRESTAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : MainController
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context, INotificator notificator):base(notificator)
        {
            _context = context;
        }

        // GET: api/Supplier
        [ClaimsAuthorize("Supplier", "rd")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            return await _context.Suppliers.ToListAsync();
        }

        // GET: api/Supplier/5
        [ClaimsAuthorize("Supplier", "rd")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Supplier>> GetSupplier(Guid id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return supplier;
        }

        // PUT: api/Supplier/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ClaimsAuthorize("Supplier", "wr")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutSupplier(Guid id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                NotifyError("Id doesn't match to the selected record.");
                return CustomResponse(supplier);
            }

            if(!ModelState.IsValid) return CustomResponse(ModelState);

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CustomResponse(supplier);
        }

        // POST: api/Supplier
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ClaimsAuthorize("Supplier", "wr")]
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            _context.Suppliers.Add(supplier);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                NotifyError(exception.InnerException.Message);
                return CustomResponse(supplier);
            }

            // return CreatedAtAction("GetSupplier", new { id = supplier.Id }, supplier);
            return CustomResponse(supplier);
        }

        // DELETE: api/Supplier/5
        [ClaimsAuthorize("Supplier", "dl")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return CustomResponse(supplier);
        }

        private bool SupplierExists(Guid id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
