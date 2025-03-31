using Infrastructure.ConfigService;
using Infrastructure.Messagers.EmailService;
using ExceptionHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Messagers.SmsService;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using WebApi.Tools.CustomIdentityError;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Helpers;
using WebApi.Tools.TokenValidator;
using Application.TokenService;
using Application.Interfaces.Localization;
using Infrastructure.Localization;
using WebApi.Filters.Language;
using Persistence.Categories.Commands;
using Application.CategoryService;
using Application.Interfaces.ConfigService;
using Application.Interfaces.Messagers.EmailService;
using Application.Interfaces.Messagers.SmsService;
using Persistence.Seeds;

var builder = WebApplication.CreateBuilder(args);

IConfiguration Configuration = builder.Configuration;

//Nlog configs
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
builder.Host.UseNLog();


// Add Localization and set their configs
builder.Services.AddLocalization(options => options.ResourcesPath = "Infrastructure/Localization");


//Add CORS configs
//Get origins cores in appsetting
var corsOrigins = Configuration.GetSection("CorsOrigins").Get<CorsPolicy>();
builder.Services.AddCors(options =>
{
    if (corsOrigins.AllowAnyOrigins) options.AddPolicy("CorsPolicy", b => b.AllowAnyOrigin());
    else options.AddPolicy("CorsPolicy", b => b.WithOrigins(corsOrigins.Origins));

    if (corsOrigins.AllowAnyMethods) options.AddPolicy("CorsPolicy", b => b.AllowAnyMethod());
    else options.AddPolicy("CorsPolicy", b => b.WithMethods(corsOrigins.Methods));

    if (corsOrigins.AllowAnyHeaders) options.AddPolicy("CorsPolicy", b => b.AllowAnyHeader());
    else options.AddPolicy("CorsPolicy", b => b.WithHeaders(corsOrigins.Headers));
});

// Add services to the container.

//Config controller service
builder.Services.AddControllers();

//Config Vesioning
builder.Services.AddApiVersioning(option =>
{
    option.ReportApiVersions = true;
});

//Config DataBase
#region Connect To DataBase
string connection = Configuration["DatabaseSettings:ConnectionString"];
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connection));
#endregion


//Config Identity and his option
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DataBaseContext>()
    .AddDefaultTokenProviders()
    .AddRoles<Role>()
    .AddErrorDescriber<CustomIdentityErrors>();

//Set Identity's Options
var identitySettings = Configuration.GetSection("IdentitySettings").Get<IdentitySettings>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = false;

    options.Password.RequireDigit = identitySettings.RequireDigit;
    options.Password.RequiredLength = identitySettings.RequiredLength;
    options.Password.RequireLowercase = identitySettings.RequireLowercase;
    options.Password.RequireUppercase = identitySettings.RequireUppercase;
    options.Password.RequireNonAlphanumeric = identitySettings.RequireNonAlphanumeric;
    options.Password.RequiredUniqueChars = identitySettings.RequiredUniqueChars;
    options.Lockout.MaxFailedAccessAttempts = identitySettings.MaxFailedAccessAttempts;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.DefaultLockoutTimeSpan);

    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = true;
    options.SignIn.RequireConfirmedAccount = false;

});
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(identitySettings.DefaultTokenProviderLifeSpan);
});

// Localization service
builder.Services.AddSingleton<ILocalizationService, LocalizationService>();

//Config service
builder.Services.AddSingleton<IConfigService, ConfigService>();

//Messagers service
builder.Services.AddScoped<ISmsService, FakeSmsService>();
builder.Services.AddScoped<IEmailService, MailKit_EmailService>();


//Services of token validator
builder.Services.AddScoped<ITokenValidator, TokenValidator>();

//Authorize and token services
builder.Services.AddScoped<IUserTokenService, UserTokenService>();

//Categories
builder.Services.AddScoped<IFacadeCategoryService, FacadeCategoryService>();

//Users
builder.Services.AddScoped<IFacadeUserService, FacadeUserService>();


// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCategoryCommandHandler).Assembly));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Config Swagger
builder.Services.AddSwaggerGen(c =>
{
    //برای نمایش استرینگی Enum ها
    //c.DescribeAllEnumsAsStrings();

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChidoSakht", Version = "v1" });

    //برای نمایش Description کنترلر ها
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebApi.ChidoSakht.xml"), true);

    //For Accept language -- Visible model
    if (Configuration.GetSection("Localization:Visible_AcceptLanguageForSwagger").Get<bool>())
        c.OperationFilter<AcceptLanguageHeaderFilter>();

    //For configure Authentication in swaager Ui
    var security = new OpenApiSecurityScheme
    {
        Name = "JWT Auth",
        Description = "توکن را وارد کنید",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(security.Reference.Id, security);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { security , new string[]{ } }
                });
});


//Config JWT Authenfication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtConfig =>
{
    jwtConfig.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = Configuration.GetSection("MainJwtAuthenticationSetting:Issuer").Get<string>(),
        ValidAudience = Configuration.GetSection("MainJwtAuthenticationSetting:Audience").Get<string>(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtInfo.SecretKey)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true
    };
    jwtConfig.SaveToken = true;
    jwtConfig.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            //Dependent service token validator
            var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidator>();

            //Enable it
            return tokenValidatorService.Execute(context);
        }
    };
});


//Service Handler
builder.Services.AddSingleton<HandlerOptions>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//DataBase Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataBaseContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();

    await DbInitializer.SeedPermissionsAsync(context, userManager, roleManager);
}

// Localization cofing
var defaultCulture = Configuration.GetSection("Localization:DefaultCulture").Get<string>() ?? "fa";
var supportedCultures = Configuration.GetSection("Localization:SupportedCultures").Get<string[]>() ?? new string[] { "fa" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(defaultCulture) // زبان پیش‌فرض فارسی
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);


//Swagger config
app.UseSwagger(c =>
{
    c.SerializeAsV2 = false;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
    c.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseDeveloperExceptionPage();
}
else
if (app.Environment.IsProduction())
{
    //For takes exeption
    app.UseMiddleware<ExceptionHandling.ExceptionHandlerMiddleware>();
}


app.UseHsts();
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
