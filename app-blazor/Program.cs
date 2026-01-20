using app_blazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

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

// IMPORTANT: enable interactive render mode
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
