//public class Bill
//{
//    public int BillId { get; set; }
//    public DateTime Date { get; set; }

//    public int CustomerId { get; set; }
//    public Customer Customer { get; set; }

//    public List<BillItem> BillItems { get; set; }

//    public decimal TotalAmount { get; set; }
//    public decimal Discount { get; set; }
//    public decimal FinalAmount { get; set; }
//}

// File: Models/Bill.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GroceryBillingSystem.Models
{
    public class Bill
    {
        public int BillId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal FinalAmount { get; set; }

        // Foreign Key
        public int CustomerId { get; set; }

        // Navigation Property
        public Customer Customer { get; set; } = new Customer();

        public List<BillItem> BillItems { get; set; } = new List<BillItem>();
    }
}
