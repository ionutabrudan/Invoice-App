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
    public class InvoicesController : Controller
    {
        private const int DueDays = 30;
        private readonly InvoiceAppDbContext _context;

        public InvoicesController(InvoiceAppDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
           var invoiceList = await _context.Invoice.Include(i => i.Customer).ToListAsync();
           return View(invoiceList);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Customer)
                .Include(i => i.Items).ThenInclude(itm => itm.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public async Task<IActionResult> Create()
        {
            var model = new Invoice();
            model.AvailableCustomers = await _context.Customer.ToListAsync();
            model.InvoiceDate = DateTime.Today;
            model.DueDate = model.InvoiceDate.AddDays(DueDays);
            return View(model);
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SerialNumber,InvoiceNumber,InvoiceDate,SelectedCustomerId")] Invoice invoice)
        {
            ModelState.Clear();
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == invoice.SelectedCustomerId);
            if (customer is null)
            {
                ModelState.AddModelError("SelectedCustomerId", "Unable to find customer");
                return View(invoice);
            }

            invoice.Customer = customer;

            if (ModelState.IsValid)
            {
                invoice.DueDate = invoice.InvoiceDate.AddDays(DueDays);
                
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            invoice.AvailableCustomers = await _context.Customer.ToListAsync();
            invoice.SelectedCustomerId = invoice.Customer.Id;

            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SerialNumber,InvoiceNumber,InvoiceDate,SelectedCustomerId")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            ModelState.Clear();
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == invoice.SelectedCustomerId);
            if (customer is null)
            {
                ModelState.AddModelError("SelectedCustomerId", "Unable to find customer");
                return View(invoice);
            }

            invoice.Customer = customer;

            if (ModelState.IsValid)
            {
                try
                {
                    invoice.DueDate = invoice.InvoiceDate.AddDays(DueDays);

                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.Id == id);
        }
    }
}
