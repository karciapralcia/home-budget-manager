using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManagerApp.Model;

namespace ExpenseManagerApp
{
    public class AppDbContext : DbContext
    {
        // DbSety reprezentują tabele w bazie danych
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Wskazanie pliku bazy danych SQLite
            optionsBuilder.UseSqlite("Data Source = \"..\\..\\..\\appdata.db\"");
        }

    }


}
