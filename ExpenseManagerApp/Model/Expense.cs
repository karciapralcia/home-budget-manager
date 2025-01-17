namespace ExpenseManagerApp.Model
{
    public class Expense
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public string? Category { get; set; }

        public DateTime Date { get; set; }

        public Expense(string description, double amount, string category, DateTime date)
        {
            Description = description;
            Amount = amount;
            Category = category;
            Date = date;
        }
        public Expense() { }

    }
}
