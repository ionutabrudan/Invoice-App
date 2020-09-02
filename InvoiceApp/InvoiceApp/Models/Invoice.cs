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
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }

        [Required]
        [DataType (DataType.Date)]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Required]
        [ForeignKey("CustomerId")]
        [Display(Name = "Customer")]
        public Customer Customer { get; set; } 

        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        [NotMapped]
        public List<Customer> AvailableCustomers { get; set; } = new List<Customer>();

        [Required]
        [NotMapped]
        [Display(Name ="Customer Id")]
        public int SelectedCustomerId { get; set; }

        



    }

}
