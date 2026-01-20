namespace app_models.Models;

public class TestingRun
{
    public string RunId { get; set; } = "";
    public string RunType { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime ExecutedOn { get; set; }
    public string Survey { get; set; } = "";
}

public class ReportEntry
{
    public string ReportId { get; set; } = "";
    public string Title { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime GeneratedOn { get; set; }
    public string ReportType { get; set; } = "";
    public string Survey { get; set; } = "";
    public string OmbDocketId { get; set; } = "";
}
