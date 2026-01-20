namespace app_models.Models;

public class OmbDocket
{
    public string DocketId { get; set; } = "";
    public string Title { get; set; } = "";
    public string OmbNumber { get; set; } = "";
    public DateTime ExpirationDate { get; set; }
    public string Status { get; set; } = "";
    public List<string> LinkedSurveys { get; set; } = new();
    public DateTime LastUpdated { get; set; }
    public string Owner { get; set; } = "";
}

public class OmbDocketApproval
{
    public string DocketId { get; set; } = "";
    public string Survey { get; set; } = "";
    public string SubmittedBy { get; set; } = "";
    public DateTime SubmittedOn { get; set; }
    public string Priority { get; set; } = "";
    public string? ReviewerComment { get; set; }
}

public class OmbDocketShare
{
    public string DocketId { get; set; } = "";
    public string RecipientEmail { get; set; } = "";
    public DateTime SharedOn { get; set; }
    public string SharedBy { get; set; } = "";
}
