using REVOPS.DevChallenge;
using REVOPS.DevChallenge.Clients;
using REVOPS.DevChallenge.Components;
using REVOPS.DevChallenge.Context;
using REVOPS.DevChallenge.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
builder.Services.AddControllers();

builder.Services.AddDbContext<ChallengeContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")!);
    options.EnableSensitiveDataLogging();
});

// Register HttpClient for TalkClient
builder.Services.AddHttpClient<ITalkClient, TalkClient>();

// Register ChatInfoService
builder.Services.AddScoped<IChatInfoService, ChatInfoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();

app.Run();
