using API.Middlewares;
using API.Hub;
using API.Hub.SubscribeSqlTableDependencies;
using API.Middlewares;
using Application.Commands.CreateNotifications;
using Application.Hub;
using Application.Mappers;
using Application.Queries.GetNotifications;
using Application.Services;
using CloudinaryDotNet;
using Domain.CommandModels;
using Domain.QueryModels;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using TableDependency.SqlClient;
using Application.DTO.MailDTO;

[assembly: ApiController]
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<fptforumQueryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QueryConnection")));
builder.Services.AddDbContext<fptforumCommandContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CommandConnection")));
// Config Swagger/Open API

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false)
    .AddNewtonsoftJson(x=>x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .WithOrigins("https://api.fptsocial.com","https://localhost:3000", "http://14.225.210.40:3000",
        "http://14.225.210.40:3000", "http://localhost:3000",
        "https://fptsocial.com", "http://localhost:8443")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
builder.Services.AddHealthChecks();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
//Config Hub and ServiceHub
builder.Services.AddSignalR();
builder.Services.AddSingleton<NotificationsHub>();
builder.Services.AddHostedService<NotificationsHostedService>();
builder.Services.AddSingleton<SubscribeNotificationsTableDependency>();

builder.Services.AddSingleton<INotificationsHubBackgroundService, NotificationsHubBackgroundService>();
builder.Services.AddSingleton<ICreateNotifications, CreateNotifications>();
builder.Services.AddSingleton<IGetNotifications,  GetNotifications>();
builder.Services.AddSingleton(typeof(ConnectionMapping<>));
builder.Services.AddSingleton<NotificationsHubBackgroundService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

string IdentityServerUrl = builder.Configuration.GetValue<string>("IdentityServer:url");
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = IdentityServerUrl;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/notificationsHub")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        // policy.RequireClaim("scope", Config.FeenServiceClientId);
    });
});
//builder.Services.ConfigurePolicy(builder.Configuration);

builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
// Register Cloudinary service
var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");
var cloudinary = new Cloudinary(new Account(
    cloudinaryConfig["CloudName"],
    cloudinaryConfig["ApiKey"],
    cloudinaryConfig["ApiSecret"]
));
builder.Services.AddSingleton(cloudinary);
builder.Services.AddSingleton<CheckingBadWord>();
builder.Configuration.AddJsonFile("notificationsMessage.json", optional: false, reloadOnChange: true);
builder.Services.AddHttpClient<ChatEngineService>();
builder.Services.AddHttpClient<FptAccountServices>();
builder.Services.AddSingleton<EmailServices>();
var app = builder.Build();
var connectionString = app.Configuration.GetConnectionString("CommandConnection");

// Kích hoạt Middleware để kiểm soát loại dữ liệu làm việc trên SwaggerUI
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
var antiforgery = app.Services.GetRequiredService<IAntiforgery>();


app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();
app.UseEndpoints(
    endpoints => 
    { 
        endpoints.MapHealthChecks("/health");
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action}/{id?}");
        endpoints.MapControllers();
    });
//Config Hub Route
app.MapHub<NotificationsHub>("notificationsHub");
app.Run();