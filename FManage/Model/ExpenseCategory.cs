using System;
namespace FManage.Model
{
    public class ExpenseCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public ExpenseCategory() { }

        public ExpenseCategory(int categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }
    }
}

