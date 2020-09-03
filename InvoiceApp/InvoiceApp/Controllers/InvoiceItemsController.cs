using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Data;
using InvoiceApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace InvoiceApp.Controllers
{
    public class InvoiceItemsController : Controller
    {
        private readonly InvoiceAppDbContext _context;

        public InvoiceItemsController(InvoiceAppDbContext context)
        {
            _context = context;
        }

        // GET: InvoiceItem/Create
        public async Task<IActionResult> Create(int id)
        {
            var invoice = await _context.Invoice
                                            .Include(i => i.Customer)
                                            .Include(i => i.Items).ThenInclude(itm => itm.Product)
                                            .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice is null)
            {
                return Redirect(Url.Action("Index", "Invoices"));
            }

            var model = new InvoiceItem();
            model.Invoice = invoice;
            model.AvailableProducts = await _context.Product.ToListAsync();

            return View(model);
        }

        // POST: InvoiceItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [FromForm][Bind("Quantity,Price,SelectedProductId")] InvoiceItem invoiceItem)
        {
            ModelState.Clear();

            var invoice = await _context.Invoice
                                            .Include(i => i.Customer)
                                            .Include(i => i.Items).ThenInclude(itm => itm.Product)
                                            .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice is null)
            {
                return Redirect(Url.Action("Index", "Invoices"));
            }

            invoiceItem.Invoice = invoice;
            invoiceItem.AvailableProducts = await _context.Product.ToListAsync();

            var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == invoiceItem.SelectedProductId);
            if (product is null)
            {
                ModelState.AddModelError("SelectedProductId", "Unable to find product");
                return View(invoice);
            }

            invoiceItem.Product = product;

            if (ModelState.IsValid)
            {
                _context.InvoiceItem.Add(invoiceItem);
                await _context.SaveChangesAsync();

                return Redirect(Url.Action("Details", "Invoices", new { invoiceItem.Invoice.Id }));
            }

            return View(invoiceItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceItem = await _context.InvoiceItem
                .Include(itm => itm.Invoice)
                .Include(itm => itm.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invoiceItem == null)
            {
                return NotFound();
            }

            return View(invoiceItem);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceItem = await _context.InvoiceItem
                .Include(itm => itm.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);

            _context.InvoiceItem.Remove(invoiceItem);
            await _context.SaveChangesAsync();

            return Redirect(Url.Action("Details", "Invoices", new { invoiceItem.Invoice.Id }));
        }
    }
}
