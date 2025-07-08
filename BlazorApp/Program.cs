using AuthExtensions;
using BlazorApp;
using BlazorApp.Components;
using BlazorApp.Components.Account;
using BlazorApp.Extensions;
using CoreNotify.MailerSend.Extensions;
using Database;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Radzen;
using Serilog;
using SerilogBlazor.Postgres;

var builder = WebApplication.CreateBuilder(args);

var connectionString = AppDbFactory.GetConnectionString();

var logLevels = new ApplicationLogLevels();
Log.Logger = logLevels
    .GetConfiguration()
    .WriteTo.PostgreSQL(connectionString, "serilog", columnOptions: PostgresColumnOptions.Default, needAutoCreateTable: true)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();
builder.Services.AddSerilog();
builder.Services.AddHttpClient();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddCurrentUserInfo<ApplicationUser>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddApplicationDb<ApplicationDbContext>(builder.Configuration);
builder.Services.AddCoreNotify<ApplicationUser>(builder.Configuration);

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();
