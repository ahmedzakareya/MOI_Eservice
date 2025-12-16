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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
//using Microsoft.OpenApi.Models;
using System.Text;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Logging
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Services.AddDistributedMemoryCache(); // For in-memory cache

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
        options.Cookie.HttpOnly = true;                // Make session cookie HTTP-only
        options.Cookie.IsEssential = true;             // Essential for session
    });
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            policy.WithOrigins("https://localhost:7214") // Admin project URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });
    builder.Services.AddHttpContextAccessor();
    #region Identity
    builder.Services.AddIdentity<AspNetUser,AspNetRole>()
                       .AddEntityFrameworkStores<EServiceDbContext>()
                       .AddDefaultTokenProviders();
    builder.Services.Configure<IdentityOptions>(options =>
                       {
                           options.User.AllowedUserNameCharacters = "0123456789";
                           
                           //options.User.RequireUniqueEmail = true;

                       });
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
    });
    #endregion
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
                    Console.WriteLine("Token validated successfully Api.");
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
                    Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                }
            };
        });

   
    builder.Services.AddAuthorization();
    #endregion
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer prefix",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        });

    //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        new string[] {}
    //    }
    //});
    });
   
    #region Database Context
    builder.Services.AddDbContext<EServiceDbContext>(options =>
        options.UseLazyLoadingProxies(false)
               .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());

    #endregion
    builder.Services.AddHttpClient();

    builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    // Add services to the container
    builder.Services.AddScoped(typeof(IUnitOfwork), typeof(UnitOfWork));
    //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<LogManager>();

    builder.Services.AddScoped<GenerateLicNo>();
    builder.Services.AddScoped(typeof(GeneralReqNo));

    builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
    builder.Services.AddScoped(typeof(IDataFetchService), typeof(DataFetchService));
    builder.Services.AddScoped(typeof(IUpdateDataService),typeof(UpdateDataService));

    builder.Services.AddScoped<MenuHelper>();
    builder.Services.AddScoped<EmailService>();

  
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    // JSON Options
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.MaxDepth = 64;
            //options.JsonSerializerOptions. = System.Text.Encoding.UTF8;
        });

    //// Authentication
    //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    //    .AddCookie(options =>
    //    {
    //        // Cookie settings for AspNetUser
    //        options.LoginPath = "/Account/Login";
    //        options.AccessDeniedPath = "/Account/AccessDenied";
    //        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    //        options.SlidingExpiration = true; // Cookie expiration and sliding logic
    //    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        //c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        //{
        //    Title = "Lube Analyst Next Gen Api",
        //    Version = "v1"
        //});

        c.CustomSchemaIds(type => type.Name);
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shell Lube Analyst Next Gen Api");

            c.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
            {
                ["activated"] = false
            };
            //c.ConfigObject.AdditionalItems["requestInterceptor"] = "function(request) { return request; }";
            c.ConfigObject.AdditionalItems["deepLinking"] = false;
            c.ConfigObject.AdditionalItems["validatorUrl"] = null;
            c.DefaultModelExpandDepth(1); // Limit model depth to avoid heavy rendering
            c.DefaultModelsExpandDepth(-1); // Hide schemas section
            c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);

            c.DisplayRequestDuration();

        });
    }
    
    app.UseHttpsRedirection();

    // Developer exception page for debugging
    app.UseDeveloperExceptionPage();
    app.UseRouting();
    app.UseCors("AllowSpecificOrigins");

    app.Use(async (context, next) =>
    {
        Console.WriteLine("Authorization Header: " + context.Request.Headers["Authorization"]);
        await next.Invoke();
    });
    app.UseSession();

    // CORS
    

    app.Use(async (context, next) =>
    {
        Console.WriteLine("Middleware Before Authentication");
        await next.Invoke();
        Console.WriteLine("Middleware After Authentication");
    });

    app.UseAuthentication();

    app.Use(async (context, next) =>
    {
        Console.WriteLine("Middleware Before Authorization");
        await next.Invoke();
        Console.WriteLine("Middleware After Authorization");
    });

    app.UseAuthorization();


    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Unhandled exception: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    Environment.Exit(1);
}
