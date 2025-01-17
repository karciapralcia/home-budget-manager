using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManagerApp
{
    internal class CategoriesManager
    {
        public static ObservableCollection<string> ExpenseCategories { get; } = new ObservableCollection<string>
    {
        "Food",
        "Transport",
        "Utilities",
        "Entertainment"
    };

        public static ObservableCollection<string> IncomeCategories { get; } = new ObservableCollection<string>
    {
        "Salary",
        "Bonus",
        "Investment"
    };
    }
}
