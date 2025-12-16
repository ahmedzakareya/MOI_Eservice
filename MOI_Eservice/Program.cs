using Business.Helpers;
using Business.Interfaces;
using Business.Mapping;
using Business.Repository;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using MOI_Eservice.MiddleWare;
using System.Text;
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddEndpointsApiExplorer(); // For API exploration/documentation
    builder.Services.AddScoped<LogManager>();
    builder.Services.AddHttpClient<HelperUrlApi>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7095/");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

    });
    builder.Services.AddScoped<PaymentGatewayService>();


    builder.Services.AddHttpClient("ApiClient", client =>
    {
        client.BaseAddress = new Uri("https://localhost:7095/");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

    })
 .AddHttpMessageHandler<TokenDelegatingHandler>();
    builder.Services.AddHttpContextAccessor();

    // Add Token Delegating Handler
    builder.Services.AddTransient<TokenDelegatingHandler>();
    builder.Services.AddLogging();

    #region Identity
    builder.Services.AddIdentity<AspNetUser, AspNetRole>()
                       .AddEntityFrameworkStores<EServiceDbContext>()
                       .AddDefaultTokenProviders();
    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.User.AllowedUserNameCharacters = "0123456789"; 
        //options.User.RequireUniqueEmail = true;

    });
    #endregion

    builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


    #region Session and CORS Configuration
    builder.Services.AddDistributedMemoryCache();

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", policy =>
        {
            policy.WithOrigins("https://localhost:7095") // Admin project URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });
    
    #region JWT
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");

    builder.Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer( options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.IncludeErrorDetails = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                

                ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
                ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
                ClockSkew = TimeSpan.Zero
            };

            // Add Event Handlers for Debugging or Custom Logic
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated successfully admin.");
                    var claimsPrincipal = context.Principal;
                    if (claimsPrincipal != null)
                    {
                        foreach (var claim in claimsPrincipal.Claims)
                        {
                            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                        }
                    }
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Authentication failed admin: {context.Exception.Message}");
                    return Task.CompletedTask;
                }
            };
        });
    //builder.Services.AddAuthentication()
    //.AddCookie("AdminScheme", options =>
    //{
    //    options.LoginPath = "/Admin/Login";
    //    options.AccessDeniedPath = "/Admin/AccessDenied";
    //    options.Cookie.Name = "AdminAuthCookie";
    //})
    //.AddCookie("UserScheme", options =>
    //{
    //    options.LoginPath = "/Home/Login";
    //    options.AccessDeniedPath = "/Home/AccessDenied";
    //    options.Cookie.Name = "UserAuthCookie";
    //});

    #endregion
    builder.Services.AddAuthorization();
    #region Dependency Injection
    builder.Services.AddScoped<IUnitOfwork, UnitOfWork>();
    builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
    builder.Services.AddScoped<MenuHelper>();
    builder.Services.AddScoped<HelperUrlApi>();
    builder.Services.AddScoped<EmailService>();
    builder.Services.AddScoped(typeof(GenerateLicNo));
    builder.Services.AddScoped(typeof(GeneralReqNo));

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
    });
    builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    #endregion
    //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    //    .AddCookie( options =>
    //    {
    //        // Cookie settings for AspNetUser
    //        options.LoginPath = "/Account/Login";
    //        options.AccessDeniedPath = "/Account/AccessDenied";
    //        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    //        options.SlidingExpiration = true; // Cookie expiration and sliding logic
    //    });

    #endregion
    #region DbContext Configuration
    builder.Services.AddDbContext<EServiceDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    #endregion
    var app = builder.Build();
    // Middleware configurations
    app.Use(async (context, next) =>
    {
        Console.WriteLine($"User IsAuthenticated: {context.User.Identity.IsAuthenticated}");
        await next();
    });
    app.UseHttpsRedirection(); 
    app.UseStaticFiles();
    app.UseRouting();
    app.UseSession();
    app.UseCors("AllowAllOrigins"); // CORS setup
    
    app.Use(async (context, next) =>
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();
        Console.WriteLine("Authorization Header: " + authHeader);
        await next.Invoke();
    });
    app.UseMiddleware<TokenHandlerMiddleware>();
    // Must come before authentication if using session
    app.UseAuthentication(); // Validate JWT
    app.UseAuthorization(); // Apply policies and claims-based access control
    app.MapControllers();

    // CORS policy
    //app.Use(async (context, next) =>
    //{
    //    var JWToken = context.Session.GetString("JWToken");
    //    if (!string.IsNullOrEmpty(JWToken))
    //    {
    //        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
    //    }
    //    await next();
    //});
    //app.Use(async (context, next) =>
    //{
    //    Console.WriteLine("Middleware Before Authentication");
    //    await next.Invoke();
    //    Console.WriteLine("Middleware After Authentication");
    //});

    //app.UseAuthentication();

    //app.Use(async (context, next) =>
    //{
    //    Console.WriteLine("Middleware Before Authorization");
    //    await next.Invoke();
    //    Console.WriteLine("Middleware After Authorization");
    //});

    //app.UseAuthorization();

    //#region Routing
    //app.MapControllers(); // Attribute-routed API controllers
    app.MapControllerRoute(
        name: "admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
   

    // Start the application
    app.Run();
}
catch(Exception ex)
{

    Console.WriteLine($"Unhandled exception: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    Environment.Exit(1);
}
