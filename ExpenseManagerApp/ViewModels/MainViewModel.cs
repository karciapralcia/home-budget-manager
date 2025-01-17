using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ExpenseManagerApp;
using ExpenseManagerApp.ViewModels;
using ExpenseManagerApp.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Linq;
using System.Diagnostics;
using ExpenseManagerApp.Model;

namespace MainViewModel.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand OpenAddExpenseWindowCommand { get; }
        public ICommand OpenAddIncomeWindowCommand { get; }
        public ICommand OpenAddExpenseCommand { get; }
        public ICommand OpenAddIncomeCommand { get; }
        public ICommand RemoveExpenseCommand { get; }
        public ICommand RemoveIncomeCommand { get; }
        public ICommand OpenAddExpensesCategoryCommand { get; }
        public ICommand OpenAddIncomeCategoryCommand { get; }

        public enum CategoryType
        {
            Expenses,
            Income
        }
        DateTime today = DateTime.Today;
        private ObservableCollection<Expense> _expenses;
        private ObservableCollection<Income> _incomes;
        private Expense _selectedExpense;
        private Income _selectedIncome;
        private double _totalIncomes;
        private double _totalExpenses;

        public MainViewModel()
        {
            Expenses = new ObservableCollection<Expense>();
            Expenses.CollectionChanged += Expenses_CollectionChanged;
            Incomes = new ObservableCollection<Income>();
            Incomes.CollectionChanged += Incomes_CollectionChanged;
            OpenAddExpenseWindowCommand = new RelayCommand(OpenAddExpenseWindow);
            OpenAddIncomeWindowCommand = new RelayCommand(OpenAddIncomeWindow);
            RemoveExpenseCommand = new RelayCommand(RemoveSelectedExpense, CanRemoveExpense);
            RemoveIncomeCommand = new RelayCommand(RemoveSelectedIncome, CanRemoveIncome);
            OpenAddExpensesCategoryCommand = new RelayCommand(() => OpenAddCategoryWindow(CategoryType.Expenses));
            OpenAddIncomeCategoryCommand = new RelayCommand(() => OpenAddCategoryWindow(CategoryType.Income));

            LoadData();
            SumOfAmountIncomes(today);
            SumOfAmountExpenses(today);
        }
        private void SumOfAmountExpenses(DateTime today)
        {
            TotalExpenses = Expenses.Where(t => t.Date.Month == today.Month).Sum(t => t.Amount);
        }
        private void SumOfAmountIncomes(DateTime today)
        {
            TotalIncomes = Incomes.Where(t => t.Date.Month == today.Month).Sum(t => t.Amount);
        }
        private void OpenAddCategoryWindow(CategoryType categoryType)
        {
            var addCategoryViewModel = new AddCategoryViewModel(categoryType);
            var addCategoryWindow = new AddCategoryWindow { DataContext = addCategoryViewModel };
            addCategoryWindow.ShowDialog();
        }

        public Expense SelectedExpense
        {
            get => _selectedExpense;
            set
            {
                _selectedExpense = value;
                OnPropertyChanged(nameof(SelectedExpense));
                if(RemoveExpenseCommand is RelayCommand relayCommand)
                {
                    relayCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public Income SelectedIncome
        {
            get => _selectedIncome;
            set
            {
                _selectedIncome = value;
                OnPropertyChanged(nameof(SelectedIncome));
                if (RemoveIncomeCommand is RelayCommand relayCommand)
                {
                    relayCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void LoadData()
        {
            using (var context = new AppDbContext())
            {
                // Tymczasowo odłącz obsługę CollectionChanged
                Expenses.CollectionChanged -= Expenses_CollectionChanged;
                Incomes.CollectionChanged -= Incomes_CollectionChanged;
                var connection = context.Database.GetDbConnection();
                var expenses = context.Expenses.ToList();
                var incomes = context.Incomes.ToList();

                foreach (var expense in expenses)
                {
                    Expenses.Add(expense);
                }

                foreach (var income in incomes)
                {
                    Incomes.Add(income);
                }
                // Ponownie podłącz obsługę CollectionChanged
                Expenses.CollectionChanged += Expenses_CollectionChanged;
                Incomes.CollectionChanged += Incomes_CollectionChanged;
            }
        }
        // Usuwanie elementu z Expense
        public void RemoveSelectedExpense()
        {
            if (SelectedExpense != null)
            {
                RemoveExpense(SelectedExpense);
                SumOfAmountExpenses(DateTime.Today);

            }
        }

        private bool CanRemoveExpense()
        {
            return SelectedExpense != null;
        }

        public void RemoveExpense(Expense expense)
        {
            if (expense != null)
            {
                // Usuń z bazy danych
                using (var context = new AppDbContext())
                {
                    context.Expenses.Remove(expense);
                    context.SaveChanges();
                }

                // Usuń z ObservableCollection
                Expenses.CollectionChanged -= Expenses_CollectionChanged; // Tymczasowo odłącz zdarzenie
                Expenses.Remove(expense);
                Expenses.CollectionChanged += Expenses_CollectionChanged; // Ponownie podłącz zdarzenie

                // Zresetuj SelectedExpense
                SelectedExpense = null;
            }
        }

        // Usuwanie elementu z Income
        public void RemoveSelectedIncome()
        {
            if (SelectedIncome != null)
            {
                RemoveIncome(SelectedIncome);
                SumOfAmountIncomes(DateTime.Today);
            }
        }
        private bool CanRemoveIncome()
        {
            return SelectedIncome != null;
        }
        public void RemoveIncome(Income income)
        {
            if (income != null)
            {
                // Usuń z bazy danych
                using (var context = new AppDbContext())
                {
                    context.Incomes.Remove(income);
                    context.SaveChanges();
                }

                // Usuń z ObservableCollection
                Incomes.CollectionChanged -= Incomes_CollectionChanged; // Tymczasowo odłącz zdarzenie
                Incomes.Remove(income);
                Incomes.CollectionChanged += Incomes_CollectionChanged; // Ponownie podłącz zdarzenie

                // Zresetuj SelectedExpense
                SelectedIncome = null;
            }
        }
        private void Expenses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (Expense newExpense in e.NewItems)
                    {
                        context.Expenses.Add(newExpense);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (Expense removedExpense in e.OldItems)
                    {
                        context.Expenses.Remove(removedExpense);
                    }
                }
                context.SaveChanges();
            }
            SumOfAmountExpenses(DateTime.Today);
        }
        private void Incomes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (Income newIncome in e.NewItems)
                    {
                        context.Incomes.Add(newIncome);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (Income removedIncome in e.OldItems)
                    {
                        context.Incomes.Remove(removedIncome);
                    }
                }
                context.SaveChanges();
            }
            SumOfAmountIncomes(DateTime.Today);
        }
        private void OpenAddExpenseWindow()
        {
            var viewModel = new AddExpenseViewModel(Expenses); // Przekazanie głównej listy Expenses
            var addExpenseWindow = new AddExpenseWindow(viewModel); // Przekazanie ViewModelu do okna
            addExpenseWindow.ShowDialog(); // Otworzenie okna modalnie
        }

        private void OpenAddIncomeWindow()
        {
            var viewModel = new AddIncomeViewModel(Incomes); // Przekazanie głównej listy Incomes
            var addExpenseWindow = new AddIncomeWindow(viewModel); // Przekazanie ViewModelu do okna
            addExpenseWindow.ShowDialog(); // Otworzenie okna modalnie
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

        public ObservableCollection<Income> Incomes
        {
            get => _incomes;
            set
            {
                _incomes = value;
                OnPropertyChanged(nameof(Incomes));
            }
        }

        public double TotalExpenses
        {
            get => _totalExpenses;
            set
            {
                _totalExpenses = value;
                OnPropertyChanged(nameof(TotalExpenses));
            }
        }

        public double TotalIncomes
        {
            get => _totalIncomes;
            set
            {
                _totalIncomes = value;
                OnPropertyChanged(nameof(TotalIncomes));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
