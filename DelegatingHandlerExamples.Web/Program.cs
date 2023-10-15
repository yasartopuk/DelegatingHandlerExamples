using DelegatingHandlerExamples.Shared.Handlers;
using DelegatingHandlerExamples.Shared.Middlewares;
using DelegatingHandlerExamples.Shared.TokenHelpers;
using DelegatingHandlerExamples.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.LoginPath = "/login";
    });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<TraceIdHandler>();
builder.Services.AddTransient<RetryHandler>();
builder.Services.AddTransient<RequestResponseLogHandler>();

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
}).AddHttpMessageHandler<RefreshTokenHandler>();

builder.Services.AddTokenProvider(options =>
{
    options.BaseAddress = new Uri("http://localhost:5000");
    options.TokenPath = "api/login";
    options.RefreshTokenPath = "api/refreshtoken";
});

builder.Services.ConfigureAll<HttpClientFactoryOptions>(options =>
{
    options.HttpMessageHandlerBuilderActions.Add(builder =>
    {
        builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<TraceIdHandler>());
        builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<RequestResponseLogHandler>());
        builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<RetryHandler>());
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.UseTraceId();

app.Run();

//builder.Services.AddSingleton<IConfigureOptions<HttpClientFactoryOptions>>(provider =>
//{
//    return new ConfigureNamedOptions<HttpClientFactoryOptions>(name: null, options =>
//    {
//        options.HttpMessageHandlerBuilderActions.Add(builder =>
//        {
//            builder.AdditionalHandlers.Add(provider.GetRequiredService<TraceIdHandler>());
//        });
//    });
//});

//static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
//{
//    int RetryCount = 3;
//    return HttpPolicyExtensions
//        .HandleTransientHttpError()
//        .OrResult(msg => msg.StatusCode == HttpStatusCode.GatewayTimeout)
//        .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
//}
