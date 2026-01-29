using REVOPS.DevChallenge;
using REVOPS.DevChallenge.Clients;
using REVOPS.DevChallenge.Components;
using REVOPS.DevChallenge.Context;
using REVOPS.DevChallenge.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços ao container.
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

// Registra o HttpClient para o TalkClient
builder.Services.AddHttpClient<ITalkClient, TalkClient>();

// Registra o ChatInfoService
builder.Services.AddScoped<IChatInfoService, ChatInfoService>();

var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // O valor padrão do HSTS é 30 dias. Você pode querer alterar isso para cenários de produção, veja https://aka.ms/aspnetcore-hsts.
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
