using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ExpenseManagerApp.Model;
using ExpenseManagerApp.Views;
namespace ExpenseManagerApp.ViewModels
{
    public class AddExpenseViewModel : INotifyPropertyChanged
    {
        private string _description;
        private double _amount;
        private string _selectedCategory;
        private DateTime _selectedDate;
        private ObservableCollection<Expense> _expenses;
        private ObservableCollection<string> _categories;

        public AddExpenseViewModel(ObservableCollection<Expense> expenses)
        {
            Expenses = expenses; // Referencja do głównej listy Expenses
            SelectedDate = DateTime.Now;
            Categories = CategoriesManager.ExpenseCategories;
            AddExpenseCommand = new RelayCommand(AddExpense);
        }

        public ICommand AddExpenseCommand { get; }
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
        public ObservableCollection<Expense> Expenses
        {
            get => _expenses;
            set
            {
                _expenses = value;
                OnPropertyChanged(nameof(Expenses));
            }
        }

        private void AddExpense()
        {
            Expenses.Add(new Expense(Description, Amount, SelectedCategory, SelectedDate));

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
