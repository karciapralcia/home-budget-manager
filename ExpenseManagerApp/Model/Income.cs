using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManagerApp.Model
{
    public class Income
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public string? Category { get; set; }

        public DateTime Date { get; set; }

        public Income(string description, double amount, string category, DateTime date)
        {
            Description = description;
            Amount = amount;
            Category = category;
            Date = date;
        }
    }
}
