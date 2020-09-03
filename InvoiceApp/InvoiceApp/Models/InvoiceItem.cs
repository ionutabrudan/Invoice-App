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

        [Display(Name = "Quantity")]
        [Required]
        [Range(typeof(decimal), "0.01", "9999999999999999.99")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Display(Name = "Unit Price")]
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
           
        [NotMapped]
        public decimal LineTotal
        { 
            get
            {
                return this.Quantity * this.Price;
            } 
        }

        [NotMapped]
        public string LineTotalFormatted
        {
            get
            {
                return $"{LineTotal:N2}";
            }
        }

        [NotMapped]
        public List<Product> AvailableProducts { get; set; } = new List<Product>();

        [Required]
        [NotMapped]
        [Display(Name = "Product Id")]
        public int SelectedProductId { get; set; }
    }
}
