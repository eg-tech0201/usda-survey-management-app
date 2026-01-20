namespace app_models.Models;

public class BudgetSummary
{
    public decimal Allocated { get; set; }
    public decimal Spent { get; set; }
    public decimal Remaining { get; set; }
}

public class BudgetLineItem
{
    public string Category { get; set; } = "";
    public decimal Allocated { get; set; }
    public decimal Spent { get; set; }
    public decimal Remaining { get; set; }
}

public class BudgetRequest
{
    public string RequestId { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Amount { get; set; }
    public string Status { get; set; } = "";
    public string RequestedBy { get; set; } = "";
    public DateTime RequestedOn { get; set; }
}
