using System.Globalization;
using CurriculoMVC.DAO;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    // Mensagens de erro do model binding em português
    var msg = options.ModelBindingMessageProvider;
    msg.SetValueIsInvalidAccessor(v => $"O valor '{v}' é inválido.");
    msg.SetValueMustBeANumberAccessor(campo => $"O campo {campo} deve ser numérico.");
    msg.SetAttemptedValueIsInvalidAccessor((v, campo) => $"O valor '{v}' não é válido para {campo}.");
});

// O DAO recebe a IConfiguration para ler a connection string
builder.Services.AddScoped<CurriculoDAO>();

var app = builder.Build();

// Fixa a cultura em pt-BR: números como "1500,50" e datas no padrão brasileiro
var ptBR = new CultureInfo("pt-BR");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ptBR),
    SupportedCultures = new List<CultureInfo> { ptBR },
    SupportedUICultures = new List<CultureInfo> { ptBR }
});

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Curriculo}/{action=Index}/{id?}");

app.Run();
