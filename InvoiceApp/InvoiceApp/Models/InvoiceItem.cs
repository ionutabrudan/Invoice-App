using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Models
{
    public class InvoiceItem
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
           
    }
}
