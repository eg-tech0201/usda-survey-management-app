# Survey Management App (Demo)

## Download & Install
- .NET 10 SDK: https://dotnet.microsoft.com/en-us/download/dotnet/10.0

confirm with `dotnet --version`

### Restore, Build & Run (from repo root)
```bash
dotnet restore
dotnet build

dotnet run --project app-blazor
```

### API-driven UI
The current demo uses native Microsoft Blazor (Razor components) and custom CSS as a lightweight baseline. Keeping the UI flexible so that DevExpress Blazor, NICE components, or CASE-aligned UI frameworks may be layered in later if desired, without having to rework application structure.

Native Blazor + Razor components + custom CSS

### Microsoft Learn references (also see `app-blazor/Components/_Imports.razor`)
- Razor components: https://learn.microsoft.com/aspnet/core/blazor/components/
- Forms and validation: https://learn.microsoft.com/aspnet/core/blazor/forms-and-validation
- Routing and NavLink: https://learn.microsoft.com/aspnet/core/blazor/routing
- JavaScript interop: https://learn.microsoft.com/aspnet/core/blazor/javascript-interoperability
- Render modes (Interactive Server): https://learn.microsoft.com/aspnet/core/blazor/components/render-modes
