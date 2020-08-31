using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Models
{
    public class Invoice
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string SerialNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string InvoiceNumber { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();


    }

}
