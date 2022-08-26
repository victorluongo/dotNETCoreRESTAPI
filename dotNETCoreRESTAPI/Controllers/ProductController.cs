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
    public class ProductController : MainController
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context, INotificator notificator):base(notificator)
        {
            _context = context;
        }

        // GET: api/Product
        [ClaimsAuthorize("Product", "rd")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {

            var product = await _context.Products.ToListAsync();

            return CustomResponse(product);
        }

        // GET: api/Product/5
        [ClaimsAuthorize("Product", "rd")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _context.Products
                 .Include(p => p.Supplier)
                 .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return CustomResponse(product);
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ClaimsAuthorize("Product", "wr")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                NotifyError("Id doesn't match to the selected record.");
                return CustomResponse(product);
            }
        
            if(!ModelState.IsValid) return CustomResponse(ModelState);    

            if (product.ImageUpload != null)
            {
                var imageName = Guid.NewGuid() + "_" + product.Image;

                if (!fileUpload(product.ImageUpload, imageName))
                {
                    return CustomResponse(ModelState);
                }

                var _product = await _context.Products.FindAsync(id);

                fileDelete (_product.Image);

                product.Image = imageName;
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CustomResponse(product);
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ClaimsAuthorize("Product", "wr")]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var imageName = Guid.NewGuid() + "_" + product.Image;

            if(!fileUpload(product.ImageUpload, imageName))
            {
                return CustomResponse(product);
            }

            product.Image = imageName;

            _context.Products.Add(product);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                NotifyError(exception.InnerException.Message);
                return CustomResponse(product);
            }

        // return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            return CustomResponse(product);
        }

        // DELETE: api/Product/5
        [ClaimsAuthorize("Product", "dl")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            fileDelete(product.Image);

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return CustomResponse(product);
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private bool fileUpload(string imageFile, string imageName)
        {
            if(string.IsNullOrEmpty(imageFile))
            {
                NotifyError("Image file not found!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(imageFile);            

            var filePath =  Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            if(System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError(string.Empty, "File already existis!");
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        private bool fileDelete(string imageName)
        {
            var filePath =  Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            if(!System.IO.File.Exists(filePath))
            {
                // NotifyError("Image file not found!");
                // return false;
            }

            System.IO.File.Delete(filePath);

            return true;
        }               
    }
}
