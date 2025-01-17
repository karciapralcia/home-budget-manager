using ExpenseManagerApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpenseManagerApp.ViewModels
{
    public class AddIncomeViewModel : INotifyPropertyChanged
    {
        private string _description;
        private double _amount;
        private string _selectedCategory;
        private DateTime _selectedDate;
        private ObservableCollection<string> _categories;
        private ObservableCollection<Income> _incomes;
        public AddIncomeViewModel(ObservableCollection<Income> incomes)
        {
            Incomes = incomes; // Referencja do głównej listy Incomes
            SelectedDate = DateTime.Now;
            Categories = CategoriesManager.IncomeCategories;
            AddIncomeCommand = new RelayCommand(AddIncome);
        }
        public ICommand AddIncomeCommand { get; }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public double Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public ObservableCollection<string> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
        public ObservableCollection<Income> Incomes
        {
            get => _incomes;
            set
            {
                _incomes = value;
                OnPropertyChanged(nameof(Incomes));
            }
        }

        private void AddIncome()
        {
            Incomes.Add(new Income(Description, Amount, SelectedCategory, SelectedDate));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
