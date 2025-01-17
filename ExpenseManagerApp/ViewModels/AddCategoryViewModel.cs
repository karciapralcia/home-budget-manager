using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MainViewModel.ViewModels.MainViewModel;

namespace ExpenseManagerApp.ViewModels
{
    class AddCategoryViewModel
    {
        public ObservableCollection<string> Categories { get; private set; }
        public string NewCategory { get; set; }
        public RelayCommand AddCategoryCommand { get; }
        private readonly CategoryType _categoryType;

        public AddCategoryViewModel(CategoryType categoryType)
        {

            // Ładowanie odpowiedniej kolekcji na podstawie typu
            Categories = categoryType == CategoryType.Expenses
                ? CategoriesManager.ExpenseCategories
                : CategoriesManager.IncomeCategories;

            AddCategoryCommand = new RelayCommand(AddCategory);
        }

        private void AddCategory()
        {
            if (!string.IsNullOrWhiteSpace(NewCategory) && !Categories.Contains(NewCategory))
            {
                Categories.Add(NewCategory);
            }
        }
    }
  
}
