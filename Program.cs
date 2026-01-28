/*
 * ============================================================
 * PONTO DE ENTRADA DA APLICAÇÃO
 * ============================================================
 */

using REVOPS.DevChallenge;
using REVOPS.DevChallenge.Clients;
using REVOPS.DevChallenge.Components;
using REVOPS.DevChallenge.Context;
using REVOPS.DevChallenge.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração básica do ASP.NET
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
builder.Services.AddControllers();

// Configura o banco com o novo nome "ContextoBanco"
builder.Services.AddDbContext<ContextoBanco>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")!);
});

// Registra Cliente e Serviço com os novos nomes amigáveis
builder.Services.AddHttpClient<IClienteTalk, ClienteTalk>();
builder.Services.AddScoped<IServicoChat, ServicoChat>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
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
