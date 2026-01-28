/*
 * ============================================================
 * PONTO DE ENTRADA DA APLICAÇÃO
 * ============================================================
 * Este é o arquivo que inicializa a aplicação web.
 * Ele configura todos os serviços necessários antes de rodar.
 * ============================================================
 */

using REVOPS.DevChallenge;
using REVOPS.DevChallenge.Clients;
using REVOPS.DevChallenge.Components;
using REVOPS.DevChallenge.Context;
using REVOPS.DevChallenge.Services;
using Microsoft.EntityFrameworkCore;

// ====================================================
// 1. CRIAÇÃO DO BUILDER
// ====================================================
// O builder é onde configuramos todos os serviços
var builder = WebApplication.CreateBuilder(args);

// ====================================================
// 2. CONFIGURAÇÃO DOS SERVIÇOS
// ====================================================

// Habilita componentes Razor com interatividade no servidor
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// Carrega as configurações do appsettings.json (token da API, etc)
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

// Permite usar controllers (API REST)
builder.Services.AddControllers();

// Configura o banco de dados SQLite
builder.Services.AddDbContext<ChallengeContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")!);
    options.EnableSensitiveDataLogging();  // Logs detalhados (só usar em dev!)
});

// Registra o cliente HTTP para chamar a API do Talk
builder.Services.AddHttpClient<ITalkClient, TalkClient>();

// Registra o serviço de busca de chats
builder.Services.AddScoped<IChatInfoService, ChatInfoService>();

// ====================================================
// 3. CONSTRUÇÃO DA APLICAÇÃO
// ====================================================
var app = builder.Build();

// ====================================================
// 4. MIDDLEWARE (processamento de requisições)
// ====================================================

// Em produção, usa tratamento de erros e segurança extra
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();  // Segurança HTTPS
}

// Redireciona HTTP para HTTPS
app.UseHttpsRedirection();

// Serve arquivos estáticos (CSS, JS, imagens)
app.UseStaticFiles();

// Configura o roteamento
app.UseRouting();

// Proteção contra CSRF
app.UseAntiforgery();

// ====================================================
// 5. MAPEAMENTO DE ROTAS
// ====================================================

// Mapeia os componentes Razor (páginas Blazor)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Mapeia os controllers (API REST)
app.MapControllers();

// ====================================================
// 6. INICIA A APLICAÇÃO
// ====================================================
app.Run();
