namespace app_models.Models;

public class AggregateSummaryRow
{
    public string Group { get; set; } = "";
    public int Count { get; set; }
    public decimal CompletionRatePercent { get; set; }
    public decimal AverageDays { get; set; }
}

public class BusinessMetadataEntry
{
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";
    public string Definition { get; set; } = "";
    public string Owner { get; set; } = "";
    public string Category { get; set; } = "";
    public string DataSources { get; set; } = "";
    public string Format { get; set; } = "";
    public string Dependencies { get; set; } = "";
    public string LinkedAssets { get; set; } = "";
    public DateTime LastUpdated { get; set; }
}
