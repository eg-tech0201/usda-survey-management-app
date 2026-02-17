using System.Globalization;

namespace app_blazor.Services;

// Assumption: "SurveyInstance" is an in-memory demo source representing a unique
// (ReferenceDate, SurveyId, SampleId) combination. Replace with real persistence later.
public sealed class SurveyInstanceService
{
    private readonly List<SurveyInstance> _instances;

    public SurveyInstanceService()
    {
        _instances = BuildSeedInstances();
    }

    public IReadOnlyList<SurveyInstance> Instances => _instances;

    public FilterOptionsResponse GetFilterOptions(DateTime? referenceDate, int? surveyId, string? sampleId)
    {
        IEnumerable<SurveyInstance> q = _instances;

        if (referenceDate.HasValue)
        {
            var date = referenceDate.Value.Date;
            q = q.Where(i => i.ReferenceDate.Date == date);
        }

        if (surveyId.HasValue)
        {
            q = q.Where(i => i.SurveyId == surveyId.Value);
        }

        if (!string.IsNullOrWhiteSpace(sampleId))
        {
            q = q.Where(i => i.SampleId.Equals(sampleId, StringComparison.OrdinalIgnoreCase));
        }

        return new FilterOptionsResponse(
            q.Select(i => i.ReferenceDate.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .Select(d => d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                .ToList(),
            q.Select(i => i.SurveyId)
                .Distinct()
                .OrderBy(id => id)
                .ToList(),
            q.Select(i => i.SampleId)
                .Distinct()
                .OrderBy(id => id)
                .ToList()
        );
    }

    public SurveyInstanceDetailResponse? GetDetail(DateTime referenceDate, int surveyId, string sampleId)
    {
        var match = _instances.FirstOrDefault(i =>
            i.ReferenceDate.Date == referenceDate.Date &&
            i.SurveyId == surveyId &&
            i.SampleId.Equals(sampleId, StringComparison.OrdinalIgnoreCase));

        if (match is null)
            return null;

        var fullRecord = BuildFullRecord(match);
        return new SurveyInstanceDetailResponse(
            match.SampleId,
            match.SampleName,
            match.SurveyId,
            match.Title,
            match.SubTitle,
            match.SurveyFrequency,
            match.Version,
            match.SurveyDate,
            match.ReferenceDate,
            match.SurveyStartDate,
            match.SurveyStopDate,
            match.HqSurveyAdmin,
            match.ProjectCode,
            match.Modes.Select(m => new ModeWindow(m.Mode, m.StartDate, m.StopDate)).ToList(),
            match.OpDomCounts.Select(c => new CountItem(c.Code, c.Definition, c.Count)).ToList(),
            match.DcmsCounts.Select(c => new CountItem(c.Code, c.Definition, c.Count)).ToList(),
            match.TotalReceived,
            match.TotalDeleted,
            match.BudgetAllocation,
            fullRecord
        );
    }

    public List<SurveyInstance> FilterInstances(DateTime? referenceDate, int? surveyId, string? sampleId)
    {
        IEnumerable<SurveyInstance> q = _instances;

        if (referenceDate.HasValue)
        {
            var date = referenceDate.Value.Date;
            q = q.Where(i => i.ReferenceDate.Date == date);
        }

        if (surveyId.HasValue)
        {
            q = q.Where(i => i.SurveyId == surveyId.Value);
        }

        if (!string.IsNullOrWhiteSpace(sampleId))
        {
            q = q.Where(i => i.SampleId.Equals(sampleId, StringComparison.OrdinalIgnoreCase));
        }

        return q.ToList();
    }

    private static List<SurveyInstance> BuildSeedInstances()
    {
        var instances = new List<SurveyInstance>
        {
            new SurveyInstance(
                SurveyId: 1001,
                SampleId: "SMP-1001-A",
                SampleName: "CY-2026 Core Sample",
                ReferenceDate: new DateTime(2026, 1, 5),
                Title: "Crop Yield Survey (CY-2026)",
                SubTitle: "Winter grain follow-up",
                SurveyFrequency: "Annual",
                Version: "v2.1",
                SurveyDate: new DateTime(2026, 1, 5),
                SurveyStartDate: new DateTime(2026, 1, 5),
                SurveyStopDate: new DateTime(2026, 3, 31),
                HqSurveyAdmin: "A. Chen",
                ProjectCode: "CY26-CORE",
                Status: "In progress",
                OmbNumber: "0535-0123",
                OmbExpires: new DateTime(2028, 6, 30),
                State: "IA",
                Region: "UMR",
                StateId: "19",
                StateAlpha: "IA",
                DcmsCodeId: "A1",
                OpDomStatusId: "Active",
                EnumeratorId: "E-101",
                EnumeratorName: "A. Rivera",
                ManagerId: "M-201",
                ManagerName: "J. Howard",
                CoachId: "C-301",
                CoachName: "L. Park",
                EnumeratorNotes: "Initial outreach complete.",
                ResponseCode: "Complete",
                Mode: "CAPI",
                Modes: new List<ModeWindow>
                {
                    new("CASI", new DateTime(2026, 1, 5), new DateTime(2026, 2, 28)),
                    new("CAPI", new DateTime(2026, 1, 10), new DateTime(2026, 3, 10)),
                    new("CATI", new DateTime(2026, 1, 20), new DateTime(2026, 3, 31)),
                    new("MAIL", new DateTime(2026, 1, 15), new DateTime(2026, 2, 20))
                },
                OpDomCounts: new List<CountItem>
                {
                    new("Active", "Open cases currently in field", 482),
                    new("Complete", "Final disposition received", 1298),
                    new("Refused", "Refusal recorded", 66)
                },
                DcmsCounts: new List<CountItem>
                {
                    new("A1", "Accepted and complete", 1044),
                    new("B2", "Partial response", 212),
                    new("C3", "Non-contact", 145)
                },
                TotalReceived: 1256,
                TotalDeleted: 24,
                BudgetAllocation: 320_000m
            ),
            new SurveyInstance(
                SurveyId: 1001,
                SampleId: "SMP-1001-B",
                SampleName: "CY-2026 Supplemental",
                ReferenceDate: new DateTime(2026, 2, 15),
                Title: "Crop Yield Survey (CY-2026)",
                SubTitle: "Late-season addendum",
                SurveyFrequency: "Annual",
                Version: "v2.2",
                SurveyDate: new DateTime(2026, 2, 15),
                SurveyStartDate: new DateTime(2026, 2, 15),
                SurveyStopDate: new DateTime(2026, 4, 15),
                HqSurveyAdmin: "M. Lewis",
                ProjectCode: "CY26-SUP",
                Status: "Not started",
                OmbNumber: "0535-0123",
                OmbExpires: new DateTime(2028, 6, 30),
                State: "IA",
                Region: "UMR",
                StateId: "19",
                StateAlpha: "IA",
                DcmsCodeId: "B2",
                OpDomStatusId: "Active",
                EnumeratorId: "E-102",
                EnumeratorName: "K. Nguyen",
                ManagerId: "M-201",
                ManagerName: "J. Howard",
                CoachId: "C-302",
                CoachName: "S. Patel",
                EnumeratorNotes: "Follow-up scheduled.",
                ResponseCode: "Pending",
                Mode: "CATI",
                Modes: new List<ModeWindow>
                {
                    new("CASI", new DateTime(2026, 2, 15), new DateTime(2026, 3, 15)),
                    new("CATI", new DateTime(2026, 2, 20), new DateTime(2026, 4, 1)),
                    new("MAIL", new DateTime(2026, 2, 25), new DateTime(2026, 3, 10))
                },
                OpDomCounts: new List<CountItem>
                {
                    new("Active", "Open cases currently in field", 312),
                    new("Complete", "Final disposition received", 418),
                    new("Refused", "Refusal recorded", 19)
                },
                DcmsCounts: new List<CountItem>
                {
                    new("A1", "Accepted and complete", 395),
                    new("B2", "Partial response", 72),
                    new("C3", "Non-contact", 57)
                },
                TotalReceived: 467,
                TotalDeleted: 6,
                BudgetAllocation: 110_000m
            ),
            new SurveyInstance(
                SurveyId: 1002,
                SampleId: "SMP-1002-A",
                SampleName: "ACP Pilot Frame",
                ReferenceDate: new DateTime(2026, 1, 8),
                Title: "Agricultural Census Pilot (ACP)",
                SubTitle: "Pilot compliance test",
                SurveyFrequency: "One-time",
                Version: "v1.0",
                SurveyDate: new DateTime(2026, 1, 8),
                SurveyStartDate: new DateTime(2026, 1, 10),
                SurveyStopDate: new DateTime(2026, 2, 28),
                HqSurveyAdmin: "D. Khan",
                ProjectCode: "ACP-PILOT",
                Status: "Blocked",
                OmbNumber: "0535-0456",
                OmbExpires: new DateTime(2027, 9, 30),
                State: "AZ",
                Region: "MTR",
                StateId: "04",
                StateAlpha: "AZ",
                DcmsCodeId: "C3",
                OpDomStatusId: "Active",
                EnumeratorId: "E-103",
                EnumeratorName: "T. Jordan",
                ManagerId: "M-202",
                ManagerName: "S. Hale",
                CoachId: "C-303",
                CoachName: "R. Kim",
                EnumeratorNotes: "Refusal noted.",
                ResponseCode: "Refused",
                Mode: "CATI",
                Modes: new List<ModeWindow>
                {
                    new("CAPI", new DateTime(2026, 1, 10), new DateTime(2026, 2, 5)),
                    new("CATI", new DateTime(2026, 1, 15), new DateTime(2026, 2, 28)),
                    new("MAIL", new DateTime(2026, 1, 12), new DateTime(2026, 1, 30))
                },
                OpDomCounts: new List<CountItem>
                {
                    new("Active", "Open cases currently in field", 112),
                    new("Complete", "Final disposition received", 225),
                    new("Refused", "Refusal recorded", 31)
                },
                DcmsCounts: new List<CountItem>
                {
                    new("A1", "Accepted and complete", 196),
                    new("B2", "Partial response", 64),
                    new("C3", "Non-contact", 52)
                },
                TotalReceived: 280,
                TotalDeleted: 9,
                BudgetAllocation: 95_000m
            ),
            new SurveyInstance(
                SurveyId: 1003,
                SampleId: "SMP-1003-A",
                SampleName: "FLS 2026 Sample",
                ReferenceDate: new DateTime(2026, 1, 11),
                Title: "Farm Labor Study (FLS)",
                SubTitle: "Quarterly labor check-in",
                SurveyFrequency: "Quarterly",
                Version: "v3.0",
                SurveyDate: new DateTime(2026, 1, 11),
                SurveyStartDate: new DateTime(2026, 1, 15),
                SurveyStopDate: new DateTime(2026, 4, 15),
                HqSurveyAdmin: "J. Patel",
                ProjectCode: "FLS-2026",
                Status: "In progress",
                OmbNumber: "0535-0789",
                OmbExpires: new DateTime(2029, 1, 31),
                State: "OR",
                Region: "PCR",
                StateId: "41",
                StateAlpha: "OR",
                DcmsCodeId: "A1",
                OpDomStatusId: "Active",
                EnumeratorId: "E-104",
                EnumeratorName: "P. Singh",
                ManagerId: "M-203",
                ManagerName: "E. Walsh",
                CoachId: "C-304",
                CoachName: "D. Foster",
                EnumeratorNotes: "Confirmed appointment.",
                ResponseCode: "Complete",
                Mode: "CASI",
                Modes: new List<ModeWindow>
                {
                    new("CASI", new DateTime(2026, 1, 15), new DateTime(2026, 3, 5)),
                    new("CATI", new DateTime(2026, 2, 1), new DateTime(2026, 4, 10))
                },
                OpDomCounts: new List<CountItem>
                {
                    new("Active", "Open cases currently in field", 338),
                    new("Complete", "Final disposition received", 742),
                    new("Refused", "Refusal recorded", 45)
                },
                DcmsCounts: new List<CountItem>
                {
                    new("A1", "Accepted and complete", 615),
                    new("B2", "Partial response", 110),
                    new("C3", "Non-contact", 89)
                },
                TotalReceived: 802,
                TotalDeleted: 18,
                BudgetAllocation: 210_000m
            ),
            new SurveyInstance(
                SurveyId: 1004,
                SampleId: "SMP-1004-A",
                SampleName: "DIS Monthly Frame",
                ReferenceDate: new DateTime(2026, 1, 15),
                Title: "Dairy Inventory Survey (DIS)",
                SubTitle: "Monthly inventory update",
                SurveyFrequency: "Monthly",
                Version: "v5.2",
                SurveyDate: new DateTime(2026, 1, 15),
                SurveyStartDate: new DateTime(2026, 1, 1),
                SurveyStopDate: new DateTime(2026, 1, 31),
                HqSurveyAdmin: "R. Gomez",
                ProjectCode: "DIS-2026-01",
                Status: "Overdue",
                OmbNumber: "0535-0321",
                OmbExpires: new DateTime(2027, 12, 31),
                State: "WI",
                Region: "GLR",
                StateId: "55",
                StateAlpha: "WI",
                DcmsCodeId: "B2",
                OpDomStatusId: "Active",
                EnumeratorId: "E-105",
                EnumeratorName: "H. Gomez",
                ManagerId: "M-204",
                ManagerName: "R. Lopez",
                CoachId: "C-305",
                CoachName: "B. Chen",
                EnumeratorNotes: "Awaiting callback.",
                ResponseCode: "Pending",
                Mode: "CAPI",
                Modes: new List<ModeWindow>
                {
                    new("CAPI", new DateTime(2026, 1, 1), new DateTime(2026, 1, 31)),
                    new("MAIL", new DateTime(2026, 1, 5), new DateTime(2026, 1, 20))
                },
                OpDomCounts: new List<CountItem>
                {
                    new("Active", "Open cases currently in field", 205),
                    new("Complete", "Final disposition received", 488),
                    new("Refused", "Refusal recorded", 22)
                },
                DcmsCounts: new List<CountItem>
                {
                    new("A1", "Accepted and complete", 421),
                    new("B2", "Partial response", 54),
                    new("C3", "Non-contact", 63)
                },
                TotalReceived: 512,
                TotalDeleted: 11,
                BudgetAllocation: 140_000m
            )
        };
        ExpandInstances(instances, 100);
        return instances;
    }

    private static void ExpandInstances(List<SurveyInstance> instances, int targetCount)
    {
        if (instances.Count >= targetCount)
            return;

        var titles = new[]
        {
            "Grain Stocks Survey",
            "Livestock Slaughter Report",
            "Hogs and Pigs Survey",
            "Acreage Report",
            "Cotton Objective Yield Survey",
            "Cattle Inventory Survey",
            "Milk Production Survey",
            "Rice Stocks Survey",
            "Vegetable Production Survey",
            "Fruit Chemical Use Survey"
        };

        var statuses = new[] { "Not started", "In progress", "Blocked", "Overdue" };
        var states = new[] { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY", "PR" };
        var modes = new[] { "CASI", "CAPI", "CATI", "MAIL" };
        var rand = new Random(42);

        var nextSurveyId = instances.Max(i => i.SurveyId) + 1;
        while (instances.Count < targetCount)
        {
            var title = titles[rand.Next(titles.Length)];
            var year = rand.Next(2025, 2028);
            var referenceDate = new DateTime(year, rand.Next(1, 13), rand.Next(1, 25));
            var surveyDate = referenceDate;
            var startDate = surveyDate.AddDays(rand.Next(0, 5));
            var stopDate = startDate.AddDays(rand.Next(20, 120));
            var surveyId = nextSurveyId++;
            var sampleId = $"SMP-{surveyId}-{(char)('A' + rand.Next(0, 3))}";
            var sampleName = $"{title.Split(' ')[0]} Frame {year}";
            var status = statuses[rand.Next(statuses.Length)];
            var mode = modes[rand.Next(modes.Length)];
            var stateAlpha = states[rand.Next(states.Length)];
            var dcmsCode = new[] { "A1", "B2", "C3" }[rand.Next(0, 3)];
            var opDomStatus = new[] { "Active", "Complete", "Refused" }[rand.Next(0, 3)];

            instances.Add(new SurveyInstance(
                SurveyId: surveyId,
                SampleId: sampleId,
                SampleName: sampleName,
                ReferenceDate: referenceDate,
                Title: $"{title} ({year})",
                SubTitle: "Standard cycle",
                SurveyFrequency: "Annual",
                Version: $"v{rand.Next(1, 4)}.{rand.Next(0, 9)}",
                SurveyDate: surveyDate,
                SurveyStartDate: startDate,
                SurveyStopDate: stopDate,
                HqSurveyAdmin: "TBD",
                ProjectCode: $"{title.Split(' ')[0].ToUpperInvariant()}-{year}",
                Status: status,
                OmbNumber: $"0535-{rand.Next(1000, 9999)}",
                OmbExpires: surveyDate.AddYears(rand.Next(2, 4)),
                State: stateAlpha,
                Region: "N/A",
                StateId: rand.Next(1, 57).ToString("D2"),
                StateAlpha: stateAlpha,
                DcmsCodeId: dcmsCode,
                OpDomStatusId: opDomStatus,
                EnumeratorId: $"E-{rand.Next(100, 999)}",
                EnumeratorName: $"Enum {rand.Next(1, 50)}",
                ManagerId: $"M-{rand.Next(100, 999)}",
                ManagerName: $"Mgr {rand.Next(1, 30)}",
                CoachId: $"C-{rand.Next(100, 999)}",
                CoachName: $"Coach {rand.Next(1, 20)}",
                EnumeratorNotes: $"Note {rand.Next(1, 8)}: {title.Split(' ')[0]} follow-up.",
                ResponseCode: new[] { "Complete", "Pending", "Refused" }[rand.Next(0, 3)],
                Mode: mode,
                Modes: new List<ModeWindow>
                {
                    new("CASI", startDate, stopDate),
                    new("CAPI", startDate.AddDays(2), stopDate.AddDays(-5)),
                    new("CATI", startDate.AddDays(5), stopDate),
                    new("MAIL", startDate.AddDays(7), stopDate.AddDays(-7))
                },
                OpDomCounts: new List<CountItem>
                {
                    new("Active", "Open cases currently in field", rand.Next(50, 600)),
                    new("Complete", "Final disposition received", rand.Next(200, 1200)),
                    new("Refused", "Refusal recorded", rand.Next(5, 80))
                },
                DcmsCounts: new List<CountItem>
                {
                    new("A1", "Accepted and complete", rand.Next(150, 1000)),
                    new("B2", "Partial response", rand.Next(20, 200)),
                    new("C3", "Non-contact", rand.Next(10, 150))
                },
                TotalReceived: rand.Next(200, 1500),
                TotalDeleted: rand.Next(0, 60),
                BudgetAllocation: rand.Next(80_000, 500_000)
            ));
        }
    }

    private static List<DetailField> BuildFullRecord(SurveyInstance instance)
    {
        var rand = CreateStableRandom(instance.SurveyId, instance.SampleId, instance.ReferenceDate);
        var countyId = rand.Next(1, 999).ToString("D3");
        var tract = rand.Next(100000, 999999).ToString();
        var subtract = rand.Next(10, 99).ToString();
        var poid = rand.Next(1000000, 9999999).ToString();
        var zip5 = rand.Next(10000, 99999).ToString();
        var zip4 = rand.Next(1000, 9999).ToString();
        var phone = $"{rand.Next(200, 989)}-{rand.Next(200, 999)}-{rand.Next(1000, 9999)}";
        var addrNumber = rand.Next(100, 9999);
        var placeName = $"{instance.StateAlpha} City {rand.Next(1, 40)}";
        var operName = $"Oper {rand.Next(100, 999)} Farms";
        var wholeName = $"Whole {rand.Next(100, 999)} Holdings";
        var respDate = instance.ReferenceDate.AddDays(rand.Next(1, 20)).ToString("yyyy-MM-dd");
        var pmcFlag = rand.Next(0, 2) == 0 ? "N" : "Y";
        var rfoFlag = rand.Next(0, 2) == 0 ? "N" : "Y";
        var mseqnum = rand.Next(1, 9999).ToString();
        var email = $"{instance.SampleId.ToLowerInvariant()}@demo.usda.gov";
        var opmail = $"PO Box {rand.Next(100, 9999)}";
        var officeNotes = $"Office note {rand.Next(1, 6)}";
        var enumNotes = instance.EnumeratorNotes;
        var sampNo = rand.Next(100000, 999999).ToString();
        var epaId = $"EPA-{rand.Next(1000, 9999)}";
        var labels = $"Label {rand.Next(1, 5)}";
        var otherField = $"Other {rand.Next(10, 99)}";
        var latitude = (30 + rand.NextDouble() * 15).ToString("0.0000", CultureInfo.InvariantCulture);
        var longitude = (-120 + rand.NextDouble() * 15).ToString("0.0000", CultureInfo.InvariantCulture);
        var surveyWebCode = $"WEB-{rand.Next(100000, 999999)}";
        var tier = $"T{rand.Next(1, 4)}";
        var xstateLink = $"XSL-{rand.Next(1000, 9999)}";
        var coordinationId = $"COORD-{rand.Next(1000, 9999)}";
        var censusVars = $"CVAR-{rand.Next(10, 99)}";

        return new List<DetailField>
        {
            new("project_code", instance.ProjectCode),
            new("sample_id", instance.SampleId),
            new("sample_name", instance.SampleName),
            new("survey_id", instance.SurveyId.ToString()),
            new("survey_title", instance.Title),
            new("survey_subtitle", instance.SubTitle),
            new("survey_date", instance.SurveyDate.ToString("yyyy-MM-dd")),
            new("state_id", instance.StateId),
            new("stratum", $"STR-{rand.Next(10, 99)}"),
            new("target_poid", $"TP-{rand.Next(10000, 99999)}"),
            new("tract", tract),
            new("subtract", subtract),
            new("poid", poid),
            new("dcms_code_id", instance.DcmsCodeId),
            new("oper_name", operName),
            new("whole_name", wholeName),
            new("addr_delivery", $"{addrNumber} {placeName} Rd"),
            new("addr_other", $"Suite {rand.Next(100, 999)}"),
            new("place_name", placeName),
            new("state_alpha", instance.StateAlpha),
            new("zip5", zip5),
            new("zip4", zip4),
            new("op_addr_delivery", $"{addrNumber + 10} {placeName} Ave"),
            new("op_address_other", $"Box {rand.Next(10, 99)}"),
            new("op_placename", placeName),
            new("op_state_alpha", instance.StateAlpha),
            new("op_zip5", zip5),
            new("op_zip4", zip4),
            new("phone_char", phone),
            new("enumerator_id", instance.EnumeratorId),
            new("enumerator_name", instance.EnumeratorName),
            new("manager_id", instance.ManagerId),
            new("manager_name", instance.ManagerName),
            new("coach_id", instance.CoachId),
            new("coach_name", instance.CoachName),
            new("respdate", respDate),
            new("pmc_mail_flag", pmcFlag),
            new("rfo_mail_flag", rfoFlag),
            new("casi_flag", instance.Modes.Any(m => m.Mode == "CASI") ? "Y" : "N"),
            new("capi_flag", instance.Modes.Any(m => m.Mode == "CAPI") ? "Y" : "N"),
            new("cati_flag", instance.Modes.Any(m => m.Mode == "CATI") ? "Y" : "N"),
            new("mail_start_date", instance.Modes.FirstOrDefault(m => m.Mode == "MAIL")?.StartDate.ToString("yyyy-MM-dd") ?? "N/A"),
            new("mail_stop_date", instance.Modes.FirstOrDefault(m => m.Mode == "MAIL")?.StopDate.ToString("yyyy-MM-dd") ?? "N/A"),
            new("casi_start_date", instance.Modes.FirstOrDefault(m => m.Mode == "CASI")?.StartDate.ToString("yyyy-MM-dd") ?? "N/A"),
            new("casi_stop_date", instance.Modes.FirstOrDefault(m => m.Mode == "CASI")?.StopDate.ToString("yyyy-MM-dd") ?? "N/A"),
            new("capi_start_date", instance.Modes.FirstOrDefault(m => m.Mode == "CAPI")?.StartDate.ToString("yyyy-MM-dd") ?? "N/A"),
            new("capi_stop_date", instance.Modes.FirstOrDefault(m => m.Mode == "CAPI")?.StopDate.ToString("yyyy-MM-dd") ?? "N/A"),
            new("cati_start_date", instance.Modes.FirstOrDefault(m => m.Mode == "CATI")?.StartDate.ToString("yyyy-MM-dd") ?? "N/A"),
            new("cati_stop_date", instance.Modes.FirstOrDefault(m => m.Mode == "CATI")?.StopDate.ToString("yyyy-MM-dd") ?? "N/A"),
            new("op_county_id", countyId),
            new("opcounty_name", $"{placeName} County"),
            new("op_dom_status_id", instance.OpDomStatusId),
            new("mseqnum", mseqnum),
            new("e_mail", email),
            new("opmail", opmail),
            new("office_notes", officeNotes),
            new("enum_notes", enumNotes),
            new("samp_no", sampNo),
            new("epa_id", epaId),
            new("label1-5", labels),
            new("other_field", otherField),
            new("latitude longitude", $"{latitude}, {longitude}"),
            new("response_code", instance.ResponseCode),
            new("state-abbrev", instance.StateAlpha),
            new("survey code for web access", surveyWebCode),
            new("tier", tier),
            new("xstatelink (casi)", xstateLink),
            new("coordination_id (capi)", coordinationId),
            new("Census variables", censusVars)
        };
    }

    private static Random CreateStableRandom(int surveyId, string sampleId, DateTime referenceDate)
    {
        var hash = StableHash(sampleId);
        var seed = HashCode.Combine(surveyId, hash, referenceDate.Year, referenceDate.Month, referenceDate.Day);
        return new Random(seed);
    }

    private static int StableHash(string input)
    {
        unchecked
        {
            int hash = 23;
            foreach (var ch in input)
            {
                hash = (hash * 31) + ch;
            }
            return hash;
        }
    }
}

public sealed record SurveyInstance(
    int SurveyId,
    string SampleId,
    string SampleName,
    DateTime ReferenceDate,
    string Title,
    string SubTitle,
    string SurveyFrequency,
    string Version,
    DateTime SurveyDate,
    DateTime SurveyStartDate,
    DateTime SurveyStopDate,
    string HqSurveyAdmin,
    string ProjectCode,
    string Status,
    string OmbNumber,
    DateTime OmbExpires,
    string State,
    string Region,
    string StateId,
    string StateAlpha,
    string DcmsCodeId,
    string OpDomStatusId,
    string EnumeratorId,
    string EnumeratorName,
    string ManagerId,
    string ManagerName,
    string CoachId,
    string CoachName,
    string EnumeratorNotes,
    string ResponseCode,
    string Mode,
    List<ModeWindow> Modes,
    List<CountItem> OpDomCounts,
    List<CountItem> DcmsCounts,
    int TotalReceived,
    int TotalDeleted,
    decimal BudgetAllocation
);

public sealed record FilterOptionsResponse(
    List<string> AvailableReferenceDates,
    List<int> AvailableSurveyIds,
    List<string> AvailableSampleIds
);

public sealed record SurveyInstanceDetailResponse(
    string SampleId,
    string SampleName,
    int SurveyId,
    string Title,
    string SubTitle,
    string SurveyFrequency,
    string Version,
    DateTime SurveyDate,
    DateTime ReferenceDate,
    DateTime SurveyStartDate,
    DateTime SurveyStopDate,
    string HqSurveyAdmin,
    string ProjectCode,
    List<ModeWindow> Modes,
    List<CountItem> OpDomCounts,
    List<CountItem> DcmsCounts,
    int TotalReceived,
    int TotalDeleted,
    decimal BudgetAllocation,
    List<DetailField> FullRecord
);

public sealed record ModeWindow(string Mode, DateTime StartDate, DateTime StopDate);

public sealed record CountItem(string Code, string Definition, int Count);

public sealed record DetailField(string Field, string Value);
