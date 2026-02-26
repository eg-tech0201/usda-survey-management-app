using app_blazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<app_blazor.Services.SurveyInstanceService>();
builder.Services.AddSingleton<app_blazor.Services.UserRecentSurveyService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/api/survey-instances/filter-options", (
    HttpRequest request,
    app_blazor.Services.SurveyInstanceService service) =>
{
    DateTime? referenceDate = null;
    if (request.Query.TryGetValue("referenceDate", out var referenceDateValue) &&
        !string.IsNullOrWhiteSpace(referenceDateValue))
    {
        if (!DateTime.TryParseExact(referenceDateValue.ToString(), "yyyy-MM-dd", null,
            System.Globalization.DateTimeStyles.None, out var parsedDate))
        {
            return Results.BadRequest("Invalid referenceDate. Use yyyy-MM-dd.");
        }

        referenceDate = parsedDate;
    }

    int? surveyId = null;
    if (request.Query.TryGetValue("surveyId", out var surveyIdValue) &&
        !string.IsNullOrWhiteSpace(surveyIdValue))
    {
        if (!int.TryParse(surveyIdValue, out var parsedId))
        {
            return Results.BadRequest("Invalid surveyId.");
        }

        surveyId = parsedId;
    }

    var sampleId = request.Query.TryGetValue("sampleId", out var sampleIdValue)
        ? sampleIdValue.ToString()
        : null;

    var response = service.GetFilterOptions(referenceDate, surveyId, sampleId);
    return Results.Ok(response);
});

app.MapGet("/api/survey-instances/detail", (
    HttpRequest request,
    app_blazor.Services.SurveyInstanceService service) =>
{
    if (!request.Query.TryGetValue("referenceDate", out var referenceDateValue) ||
        string.IsNullOrWhiteSpace(referenceDateValue) ||
        !DateTime.TryParseExact(referenceDateValue.ToString(), "yyyy-MM-dd", null,
            System.Globalization.DateTimeStyles.None, out var referenceDate))
    {
        return Results.BadRequest("referenceDate is required (yyyy-MM-dd).");
    }

    if (!request.Query.TryGetValue("surveyId", out var surveyIdValue) ||
        string.IsNullOrWhiteSpace(surveyIdValue) ||
        !int.TryParse(surveyIdValue, out var surveyId))
    {
        return Results.BadRequest("surveyId is required.");
    }

    if (!request.Query.TryGetValue("sampleId", out var sampleIdValue) ||
        string.IsNullOrWhiteSpace(sampleIdValue))
    {
        return Results.BadRequest("sampleId is required.");
    }

    var detail = service.GetDetail(referenceDate, surveyId, sampleIdValue!);
    return detail is null ? Results.NotFound() : Results.Ok(detail);
});

app.MapGet("/api/survey-instances/export", (
    HttpRequest request,
    app_blazor.Services.SurveyInstanceService service) =>
{
    DateTime? referenceDate = null;
    if (request.Query.TryGetValue("referenceDate", out var referenceDateValue) &&
        !string.IsNullOrWhiteSpace(referenceDateValue))
    {
        if (!DateTime.TryParseExact(referenceDateValue.ToString(), "yyyy-MM-dd", null,
            System.Globalization.DateTimeStyles.None, out var parsedDate))
        {
            return Results.BadRequest("Invalid referenceDate. Use yyyy-MM-dd.");
        }

        referenceDate = parsedDate;
    }

    int? surveyId = null;
    if (request.Query.TryGetValue("surveyId", out var surveyIdValue) &&
        !string.IsNullOrWhiteSpace(surveyIdValue))
    {
        if (!int.TryParse(surveyIdValue, out var parsedId))
        {
            return Results.BadRequest("Invalid surveyId.");
        }

        surveyId = parsedId;
    }

    var sampleId = request.Query.TryGetValue("sampleId", out var sampleIdValue)
        ? sampleIdValue.ToString()
        : null;

    var rows = service.FilterInstances(referenceDate, surveyId, sampleId)
        .OrderBy(r => r.SurveyId)
        .ToList();

    var sb = new System.Text.StringBuilder();
    sb.AppendLine("project_code,sample_id,sample_name,survey_id,survey_title,survey_subtitle,survey_date,state_id,state_alpha,op_dom_status_id,dcms_code_id,enumerator_id,enumerator_name,manager_id,manager_name,coach_id,coach_name,enum_notes,response_code");
    foreach (var row in rows)
    {
        sb.AppendLine(string.Join(",",
            Csv(row.ProjectCode),
            Csv(row.SampleId),
            Csv(row.SampleName),
            Csv(row.SurveyId.ToString()),
            Csv(row.Title),
            Csv(row.SubTitle),
            Csv(row.SurveyDate.ToString("yyyy-MM-dd")),
            Csv(row.StateId),
            Csv(row.StateAlpha),
            Csv(row.OpDomStatusId),
            Csv(row.DcmsCodeId),
            Csv(row.EnumeratorId),
            Csv(row.EnumeratorName),
            Csv(row.ManagerId),
            Csv(row.ManagerName),
            Csv(row.CoachId),
            Csv(row.CoachName),
            Csv(row.EnumeratorNotes),
            Csv(row.ResponseCode)
        ));
    }

    return Results.Text(sb.ToString(), "text/csv");
});

app.MapGet("/api/survey-instances/detail-export", (
    HttpRequest request,
    app_blazor.Services.SurveyInstanceService service) =>
{
    if (!request.Query.TryGetValue("referenceDate", out var referenceDateValue) ||
        string.IsNullOrWhiteSpace(referenceDateValue) ||
        !DateTime.TryParseExact(referenceDateValue.ToString(), "yyyy-MM-dd", null,
            System.Globalization.DateTimeStyles.None, out var referenceDate))
    {
        return Results.BadRequest("referenceDate is required (yyyy-MM-dd).");
    }

    if (!request.Query.TryGetValue("surveyId", out var surveyIdValue) ||
        string.IsNullOrWhiteSpace(surveyIdValue) ||
        !int.TryParse(surveyIdValue, out var surveyId))
    {
        return Results.BadRequest("surveyId is required.");
    }

    if (!request.Query.TryGetValue("sampleId", out var sampleIdValue) ||
        string.IsNullOrWhiteSpace(sampleIdValue))
    {
        return Results.BadRequest("sampleId is required.");
    }

    var detail = service.GetDetail(referenceDate, surveyId, sampleIdValue!);
    if (detail is null)
        return Results.NotFound();

    var sb = new System.Text.StringBuilder();
    sb.AppendLine("field,value");
    foreach (var field in detail.FullRecord)
    {
        sb.AppendLine($"{Csv(field.Field)},{Csv(field.Value)}");
    }

    return Results.Text(sb.ToString(), "text/csv");
});

// IMPORTANT: enable interactive render mode
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

static string Csv(string? value)
{
    if (string.IsNullOrEmpty(value))
        return "";

    var needsQuotes = value.Contains(',') || value.Contains('"') || value.Contains('\n');
    if (!needsQuotes)
        return value;

    return $"\"{value.Replace("\"", "\"\"")}\"";
}

app.Run();
