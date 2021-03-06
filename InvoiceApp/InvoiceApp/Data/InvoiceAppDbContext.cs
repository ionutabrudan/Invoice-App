﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Models;

namespace InvoiceApp.Data
{
    public class InvoiceAppDbContext : DbContext
    {
        public InvoiceAppDbContext (DbContextOptions<InvoiceAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<InvoiceApp.Models.Customer> Customer { get; set; }

        public DbSet<InvoiceApp.Models.Product> Product { get; set; }

        public DbSet<InvoiceApp.Models.Invoice> Invoice { get; set; }

        public DbSet<InvoiceApp.Models.InvoiceItem> InvoiceItem { get; set; }
    }
}
